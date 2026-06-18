using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Ambient.Backend.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public class Sprite : ISprite<BitmapSource>, IAsset
{
	protected BitmapImage? _initialImage;
	protected BitmapSource _bitmapSource;

	public BitmapSource Source => _bitmapSource;

	public Sprite()
	{
		_initialImage = new BitmapImage();
		_bitmapSource = _initialImage;
	}

	public Sprite(BitmapSource source)
	{
		_initialImage = null;
		_bitmapSource = source;
	}

	public void Load(byte[] buffer)
	{
		using var stream = new MemoryStream(buffer);

		var image = _initialImage ?? new BitmapImage();
		image.BeginInit();

		image.CacheOption = BitmapCacheOption.OnLoad;
		image.StreamSource = stream;

		image.EndInit();
		image.Freeze();

		_initialImage = null;
		_bitmapSource = Normalize(image);
	}

	public Sprite[] Split(int frameWidth, int frameHeight)
	{
		int columns = _bitmapSource.PixelWidth / frameWidth;
		int rows = _bitmapSource.PixelHeight / frameHeight;

		var frames = new Sprite[columns * rows];
		int index = 0;

		for (int y = 0; y < _bitmapSource.PixelHeight; y += frameHeight)
		{
			for (int x = 0; x < _bitmapSource.PixelWidth; x += frameWidth)
			{
				var rectangle = new Int32Rect(x, y, frameWidth, frameHeight);
				var cropped = new CroppedBitmap(_bitmapSource, rectangle);
				var sprite = new Sprite(cropped);

				frames[index++] = sprite;
			}
		}
		return frames;
	}

	public static BitmapSource Normalize(BitmapSource source)
	{
		const double DPI = 96.0;

		if (source.DpiX != DPI || source.DpiY != DPI)
		{
			var normalized = BitmapSource.Create(
				source.PixelWidth,
				source.PixelHeight,
				DPI,
				DPI,
				source.Format,
				source.Palette,
				GetPixels(source),
				GetStride(source)
			);
			normalized.Freeze();

			return normalized;
		}
		else return source;
	}

	public static byte[] GetPixels(BitmapSource source)
	{
		int stride = GetStride(source);
		byte[] pixels = new byte[stride * source.PixelHeight];

		source.CopyPixels(pixels, stride, 0);

		return pixels;
	}

	private static int GetStride(BitmapSource source)
	{
		int bytesPerPixel = (source.Format.BitsPerPixel + 7) / 8;
		return source.PixelWidth * bytesPerPixel;
	}
}
