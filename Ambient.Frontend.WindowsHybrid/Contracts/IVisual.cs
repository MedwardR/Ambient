using System.Windows.Media;
using Ambient.Backend.Contracts;

namespace Ambient.Frontend.WindowsHybrid.Contracts;

public interface IVisual : ITransformable
{
	IGraphic Graphics { get; }

	Matrix GetRenderMatrix();
}
