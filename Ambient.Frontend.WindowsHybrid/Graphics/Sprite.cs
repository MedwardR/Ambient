using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Ambient.Backend.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public class Sprite : IAsset
{
	public BitmapSource Source { get; protected set; }

	public Sprite() => Source = new BitmapImage();

	public Sprite(BitmapSource source) => Source = source;

	public void Load(byte[] buffer)
	{
		using var stream = new MemoryStream(buffer);

		var bitmap = new BitmapImage();
		bitmap.BeginInit();

		bitmap.CacheOption = BitmapCacheOption.OnLoad;
		bitmap.StreamSource = stream;

		bitmap.EndInit();
		bitmap.Freeze();

		Source = bitmap;
	}

	public Sprite[] Split(int frameWidth, int frameHeight)
	{
		int columns = Source.PixelWidth / frameWidth;
		int rows = Source.PixelHeight / frameHeight;

		var frames = new Sprite[columns * rows];
		int index = 0;

		for (int y = 0; y < Source.PixelHeight; y += frameHeight)
		{
			for (int x = 0; x < Source.PixelWidth; x += frameWidth)
			{
				var rectangle = new Int32Rect(x, y, frameWidth, frameHeight);
				var cropped = new CroppedBitmap(Source, rectangle);
				var sprite = new Sprite(cropped);

				frames[index++] = sprite;
			}
		}
		return frames;
	}
}
