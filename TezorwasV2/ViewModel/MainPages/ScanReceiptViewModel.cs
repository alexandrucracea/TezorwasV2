
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using TesseractOcrMaui;
using TesseractOcrMaui.Results;
using TezorwasV2.DTO;
using TezorwasV2.Helpers;
using TezorwasV2.Model;
using TezorwasV2.Services;
using TezorwasV2.View;
using TezorwasV2.View.AppPages;


namespace TezorwasV2.ViewModel.MainPages
{
    public partial class ScanReceiptViewModel : ObservableObject
    {
        private readonly IGlobalContext _globalContext;
        private readonly IProfileService _profileService;
        private readonly ILoadingSpinnerPopupService _popupService;
        private readonly IGptService _gptService;
        private readonly OcrService _ocrService;

        ITesseract Tesseract { get; }
        private LoadingSpinnerPopup _loadingPopup = new LoadingSpinnerPopup();

        public ScanReceiptViewModel(IGlobalContext globalContext, IProfileService profileService, ITesseract tesseract, IGptService gptService, ILoadingSpinnerPopupService popupService)
        {
            _globalContext = globalContext;
            _profileService = profileService;
            Tesseract = tesseract;
            _gptService = gptService;
            _ocrService = new OcrService();
            _popupService = popupService;
        }

        [RelayCommand]
        public async Task TakePhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo != null)
                {
                    // Save the file into local storage.
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    // Reduce the size of the image.
                    using Stream sourceStream = await photo.OpenReadAsync();
                    using SKBitmap sourceBitmap = SKBitmap.Decode(sourceStream);
                    int height = Math.Min(794, sourceBitmap.Height);
                    int width = Math.Min(794, sourceBitmap.Width);

                    using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);
                    using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);

                    using (SKData data = scaledImage.Encode())
                    {
                        File.WriteAllBytes(localFilePath, data.ToArray());
                    }

                    #region v1 - Tesseract
                    _popupService.ShowPopup(_loadingPopup);
                    //var result = await Tesseract.RecognizeTextAsync(localFilePath);
                    var result = await _ocrService.ExtractTextFromImageAsync(localFilePath);
                    //string resultLabel;
                    //if (result.NotSuccess())
                    //{
                    //    resultLabel = $"Recognition failed: {result.Status}";
                    //    return;
                    //}
                    //resultLabel = result.RecognisedText;
                    #endregion
               
                    if(result.Length > 0)
                    {
                        var generatedTaskMessage = await _gptService.GenerateReceiptTasks(result, _globalContext.UserToken);
                        var generatedTasksParsed = GptObjectParser.ParseGptReceiptTasksModel(generatedTaskMessage);
                        //ImageModel model = new ImageModel() { ImagePath = localFilePath, Title = "sample", Description = "Cool" };

                        await UpdateProfileReceipts(generatedTasksParsed);
                        await GoToReceipt(generatedTasksParsed);

                    }
                    _popupService.ClosePopup(_loadingPopup);

                    var snackbarOptions = new SnackbarOptions
                    {
                        BackgroundColor = Color.FromRgb(244, 214, 210),
                        TextColor = Color.FromRgb(150, 25, 17),
                        CornerRadius = new CornerRadius(40),
                        Font = Microsoft.Maui.Font.SystemFontOfSize(14)

                    };
                    var snackbar = Snackbar.Make("Photo does not contain text", null, actionButtonText: "", duration: new TimeSpan(0, 0, 3), snackbarOptions);

                    await snackbar.Show(default);
                }
            }
        }

        [RelayCommand]
        public async Task UploadPhoto()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null)
                {
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    try
                    {
                        // Copy the photo to local storage
                        using (Stream sourceStream = await photo.OpenReadAsync())
                        {
                            using (FileStream localFileStream = File.Create(localFilePath))
                            {
                                await sourceStream.CopyToAsync(localFileStream);
                            }
                        }

                        Console.WriteLine($"Local file path: {localFilePath}");

                        // Ensure the file exists
                        if (!File.Exists(localFilePath))
                        {
                            Console.WriteLine($"Local file not found: {localFilePath}");
                            throw new FileNotFoundException($"Local file not found: {localFilePath}");
                        }

                        Console.WriteLine("Local file exists, proceeding with OCR.");


                        _popupService.ShowPopup(_loadingPopup);
                        // Extract text from image
                        string resultLabel = await _ocrService.ExtractTextFromImageAsync(localFilePath);
                        if (resultLabel.Length > 0) { 
                            var generatedReceiptMessage = await _gptService.GenerateReceiptTasks(resultLabel, _globalContext.UserToken);
                        var generatedReceiptParsed = GptObjectParser.ParseGptReceiptTasksModel(generatedReceiptMessage);


                        await UpdateProfileReceipts(generatedReceiptParsed);
                        await GoToReceipt(generatedReceiptParsed);
                        }
                        _popupService.ClosePopup(_loadingPopup);

                        var snackbarOptions = new SnackbarOptions
                        {
                            BackgroundColor = Color.FromRgb(244, 214, 210),
                            TextColor = Color.FromRgb(150, 25, 17),
                            CornerRadius = new CornerRadius(40),
                            Font = Microsoft.Maui.Font.SystemFontOfSize(14)

                        };
                        var snackbar = Snackbar.Make("Photo does not contain text", null, actionButtonText: "", duration: new TimeSpan(0, 0, 3), snackbarOptions);

                        await snackbar.Show(default);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing the image: {ex.Message}");
                        // Handle the exception as needed
                    }
                }
            }
        }


        public void PreprocessImage(string inputImagePath, string outputImagePath)
        {
            try
            {
                // Check if the input file exists
                if (!File.Exists(inputImagePath))
                {
                    Console.WriteLine($"Input file not found: {inputImagePath}");
                    throw new FileNotFoundException($"Input file not found: {inputImagePath}");
                }

                using (var inputStream = File.OpenRead(inputImagePath))
                {
                    using (var original = SKBitmap.Decode(inputStream))
                    {
                        if (original == null)
                        {
                            Console.WriteLine("Failed to decode the input image.");
                            throw new Exception("Failed to decode the input image.");
                        }

                        // Convert to grayscale
                        using (var grayScaleBitmap = new SKBitmap(original.Width, original.Height, SKColorType.Gray8, SKAlphaType.Opaque))
                        {
                            using (var canvas = new SKCanvas(grayScaleBitmap))
                            {
                                var paint = new SKPaint
                                {
                                    ColorFilter = SKColorFilter.CreateColorMatrix(new float[]
                                    {
                                0.2126f, 0.7152f, 0.0722f, 0, 0,
                                0.2126f, 0.7152f, 0.0722f, 0, 0,
                                0.2126f, 0.7152f, 0.0722f, 0, 0,
                                0, 0, 0, 1, 0
                                    })
                                };
                                canvas.DrawBitmap(original, new SKRect(0, 0, original.Width, original.Height), paint);
                            }

                            // Apply binary thresholding
                            for (int y = 0; y < grayScaleBitmap.Height; y++)
                            {
                                for (int x = 0; x < grayScaleBitmap.Width; x++)
                                {
                                    var color = grayScaleBitmap.GetPixel(x, y);
                                    var intensity = color.Red; // Since it's grayscale, red, green, and blue are equal
                                    var binaryColor = intensity > 128 ? (byte)255 : (byte)0;
                                    grayScaleBitmap.SetPixel(x, y, new SKColor(binaryColor, binaryColor, binaryColor));
                                }
                            }

                            // Save the preprocessed image
                            using (var image = SKImage.FromBitmap(grayScaleBitmap))
                            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                            {
                                using (var outputStream = File.Create(outputImagePath))
                                {
                                    data.SaveTo(outputStream);
                                }
                            }

                            // Verifică dacă fișierul a fost creat
                            if (File.Exists(outputImagePath))
                            {
                                Console.WriteLine($"Processed file successfully created: {outputImagePath}");
                            }
                            else
                            {
                                Console.WriteLine($"Processed file not found after creation: {outputImagePath}");
                                throw new Exception($"Processed file not found after creation: {outputImagePath}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }

        private async Task UpdateProfileReceipts(dynamic generatedTasksParsed)
        {
            ProfileDto profileToUpdate = await _profileService.GetProfileInfo(_globalContext.ProfileId, _globalContext.UserToken);
            profileToUpdate.Receipts.Add(generatedTasksParsed);

            await _profileService.UpdateAProfile(profileToUpdate, _globalContext.UserToken);
        }
        public async Task GoToReceipt(dynamic receiptToTransfer)
        {
            var navigationParameters = new Dictionary<string, dynamic>
            {
                { "ReceiptToShow",receiptToTransfer},
            };
            await Shell.Current.GoToAsync(nameof(ReceiptItemView), true, navigationParameters);
        }
    }
}
