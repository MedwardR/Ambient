using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ambient.Backend.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Graphics;

public class Sprite : ISprite<ImageSource>, IAsset
{
	protected BitmapImage? _initialImage;
	protected BitmapSource _bitmapSource;

	public ImageSource Source => _bitmapSource;

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
		_bitmapSource = image;
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
}
