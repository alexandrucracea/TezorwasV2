using System;
using System.IO;
using System.Threading;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using SkiaSharp;
using Syncfusion.Maui.Core.Carousel;
using TesseractOcrMaui;
using TesseractOcrMaui.Results;
using TezorwasV2.Model;
using Microsoft.Maui.Controls;
using TezorwasV2.Services;
using TezorwasV2.Helpers;
using TesseractOcrMaui.Enums;
using TezorwasV2.ViewModel.MainPages;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using TezorwasV2.Helpers.Messages;

namespace TezorwasV2.View.AppPages
{
    public partial class ScanReceiptView : ContentPage
    {
        //ITesseract Tesseract { get; }
        //private readonly IGlobalContext _globalContext;
        ScanReceiptViewModel viewModel;
        LoadingSpinnerPopup popup;

        public ScanReceiptView(ScanReceiptViewModel scanReceiptViewModel)
        {
            InitializeComponent();
            viewModel = scanReceiptViewModel;
            BindingContext = viewModel;
        }


        private void ShowLoadingPopup()
        {
            // Show your custom loading popup
            popup = new LoadingSpinnerPopup();
            this.ShowLoadingPopup();

        }

        private void HideLoadingPopup()
        {
            popup.Close();
        }
        protected override void OnAppearing()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            this.Behaviors.Add(new StatusBarBehavior
            {
                StatusBarColor = Color.FromArgb("#eff1f3"),
                StatusBarStyle = StatusBarStyle.DarkContent
            });
#pragma warning restore CA1416 // Validate platform compatibility
            base.OnAppearing();
        }


        #region test
        //private void OnCameraClicked(object sender, EventArgs e)
        //{
        //    TakePhoto();
        //}

        //// Open camera and take photo
        //public async void TakePhoto()
        //{
        //    if (MediaPicker.Default.IsCaptureSupported)
        //    {
        //        FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
        //        if (photo != null)
        //        {
        //            // Save the file into local storage.
        //            string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

        //            // Reduce the size of the image.
        //            using Stream sourceStream = await photo.OpenReadAsync();
        //            using SKBitmap sourceBitmap = SKBitmap.Decode(sourceStream);
        //            int height = Math.Min(794, sourceBitmap.Height);
        //            int width = Math.Min(794, sourceBitmap.Width);

        //            using SKBitmap scaledBitmap = sourceBitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);
        //            using SKImage scaledImage = SKImage.FromBitmap(scaledBitmap);

        //            using (SKData data = scaledImage.Encode())
        //            {
        //                File.WriteAllBytes(localFilePath, data.ToArray());
        //            }

        //            #region v1 - Tesseract

        //            var result = await Tesseract.RecognizeTextAsync(localFilePath);
        //            string resultLabel;
        //            if (result.NotSuccess())
        //            {
        //                resultLabel = $"Recognition failed: {result.Status}";
        //                return;
        //            }
        //            resultLabel = result.RecognisedText;
        //            // Create model and add to the collection
        //            #endregion
        //            ImageModel model = new ImageModel() { ImagePath = localFilePath, Title = "sample", Description = "Cool" };
        //        }
        //    }
        //}

        //private async void UploadBtn_Clicked(object sender, EventArgs e)
        //{
        //    if (MediaPicker.Default.IsCaptureSupported)
        //    {
        //        FileResult photo = await MediaPicker.Default.PickPhotoAsync();

        //        if (photo != null)
        //        {
        //            // Save the file into local storage.
        //            string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
        //            string processedFilePath = Path.Combine(FileSystem.CacheDirectory, "processed_" + photo.FileName);

        //            using Stream sourceStream = await photo.OpenReadAsync();
        //            using FileStream localFileStream = File.OpenWrite(localFilePath);

        //            await sourceStream.CopyToAsync(localFileStream);

        //            PreprocessImage(localFilePath, processedFilePath);

        //            #region v1 - Tesseract
        //            var result = await Tesseract.RecognizeTextAsync(localFilePath);
        //            string resultLabel;
        //            if (result.NotSuccess())
        //            {
        //                resultLabel = $"Recognition failed: {result.Status}";
        //                return;
        //            }
        //            resultLabel = result.RecognisedText;
        //            // Create model and add to the collection
        //            #endregion

        //            IGptService gptService = new GptService();
        //            var x = await gptService.GenerateReceiptTasks(resultLabel,_globalContext.UserToken);
        //            var y = GptObjectParser.ParseGptReceiptTasksModel(x);
        //            ImageModel model = new ImageModel() { ImagePath = localFilePath, Title = "sample", Description = "Cool" };
        //        }
        //    }
        //}

        //public void PreprocessImage(string inputImagePath, string outputImagePath)
        //{
        //    int retryCount = 5;
        //    int delay = 500; // 500 milliseconds delay between retries

        //    while (retryCount > 0)
        //    {
        //        try
        //        {
        //            // Check if the input file exists
        //            if (!File.Exists(inputImagePath))
        //            {
        //                throw new FileNotFoundException($"Input file not found: {inputImagePath}");
        //            }

        //            using (var inputStream = File.OpenRead(inputImagePath))
        //            {
        //                using (var original = SKBitmap.Decode(inputStream))
        //                {
        //                    if (original == null)
        //                    {
        //                        throw new Exception("Failed to decode the input image.");
        //                    }

        //                    // Convert to grayscale
        //                    using (var grayScaleBitmap = new SKBitmap(original.Width, original.Height, SKColorType.Gray8, SKAlphaType.Opaque))
        //                    {
        //                        using (var canvas = new SKCanvas(grayScaleBitmap))
        //                        {
        //                            var paint = new SKPaint
        //                            {
        //                                ColorFilter = SKColorFilter.CreateColorMatrix(new float[]
        //                                {
        //                                    0.2126f, 0.7152f, 0.0722f, 0, 0,
        //                                    0.2126f, 0.7152f, 0.0722f, 0, 0,
        //                                    0.2126f, 0.7152f, 0.0722f, 0, 0,
        //                                    0, 0, 0, 1, 0
        //                                })
        //                            };
        //                            canvas.DrawBitmap(original, new SKRect(0, 0, original.Width, original.Height), paint);
        //                        }

        //                        // Apply binary thresholding
        //                        for (int y = 0; y < grayScaleBitmap.Height; y++)
        //                        {
        //                            for (int x = 0; x < grayScaleBitmap.Width; x++)
        //                            {
        //                                var color = grayScaleBitmap.GetPixel(x, y);
        //                                var intensity = color.Red; // Since it's grayscale, red, green, and blue are equal
        //                                var binaryColor = intensity > 128 ? (byte)255 : (byte)0;
        //                                grayScaleBitmap.SetPixel(x, y, new SKColor(binaryColor, binaryColor, binaryColor));
        //                            }
        //                        }

        //                        // Save the preprocessed image
        //                        using (var image = SKImage.FromBitmap(grayScaleBitmap))
        //                        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        //                        using (var outputStream = File.OpenWrite(outputImagePath))
        //                        {
        //                            data.SaveTo(outputStream);
        //                        }
        //                    }
        //                }
        //            }

        //            // If the process is successful, break out of the retry loop
        //            break;
        //        }
        //        catch (IOException ex) when (retryCount > 0)
        //        {
        //            // Log the exception or handle it accordingly
        //            Console.WriteLine($"File is in use, retrying... ({retryCount} retries left)");

        //            // Decrement the retry count and wait before retrying
        //            retryCount--;
        //            Thread.Sleep(delay);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log the exception or handle it accordingly
        //            Console.WriteLine($"An error occurred: {ex.Message}");
        //            break;
        //        }
        //    }
        //}
        #endregion
    }
}
