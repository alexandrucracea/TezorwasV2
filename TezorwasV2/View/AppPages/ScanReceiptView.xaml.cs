using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using SkiaSharp;
using System.Reflection;
using TesseractOcrMaui;
using TesseractOcrMaui.Results;
using TezorwasV2.Model;

namespace TezorwasV2.View.AppPages;

public partial class ScanReceiptView : ContentPage
{
    ITesseract Tesseract { get; }

    public ScanReceiptView(ITesseract tesseract)
    {
        InitializeComponent();
        Tesseract = tesseract;

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
    private void OnCameraClicked(object sender, EventArgs e)
    {
        TakePhoto();
    }
    //Open camera and take photo
    public async void TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo != null)
            {
                // Save the file into local storage.
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                //Reduce the size of the image. 
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

                var result = await Tesseract.RecognizeTextAsync(localFilePath);
                string resultLabel;
                if (result.NotSuccess())
                {
                    resultLabel = $"Recognizion failed: {result.Status}";
                    return;
                }
                resultLabel = result.RecognisedText;
                //Create model and add to the collection
                ImageModel model = new ImageModel() { ImagePath = localFilePath, Title = "sample", Description = "Cool" };
            }
        }
    }
}