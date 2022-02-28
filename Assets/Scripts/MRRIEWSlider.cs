using UnityEngine;
using UnityEngine.UI;


	public class MRRIEWSlider : UnityEngine.UI.Slider
	{

		Image fill;
		Image handle;

		public void setColors(Color a)
		{

			if (fill == null) fill = transform.Find("Fill Area/Fill").GetComponent<Image>();
			
			if (handle == null) handle = transform.Find("Handle Slide Area/Handle").GetComponent<Image>();
			

			fill.color = a;

			handle.color = a;
		}

	}

