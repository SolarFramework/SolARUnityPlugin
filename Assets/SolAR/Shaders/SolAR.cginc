/*
 public enum ImageLayout {
	LAYOUT_RGB = 0,
	LAYOUT_GRB,
	LAYOUT_BGR,
	LAYOUT_GREY,
	LAYOUT_RGBA,
	LAYOUT_RGBX,
	LAYOUT_UNDEFINED
 }
*/

/*
		[Enum(RGB,0,GRB,1,BGR,2,GREY,3,RGBA,4,RGBX,5,UNDEFINED,6)]
		_Layout("Layout", Int) = 0
*/

/*
		[Enum(RGB,0,GRB,1,BGR,2)]
		_Layout("Layout", Int) = 0
*/

int _Layout;

fixed4 SolAR_Convert(fixed4 color)
{
	switch (_Layout)
	{
	case 1: // LAYOUT_GRB
		return color.grba;
	case 2: // LAYOUT_BGR
		return color.bgra;
	default:
		return color.rgba;
	}
}
