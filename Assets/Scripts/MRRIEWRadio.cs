using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


	public class  MRRIEWRadio : UnityEngine.UI.Toggle
	{

		Color accentColor = Color.white;
		Color outlineColor = Color.white;
		Image outline;
		Image dot;
		public ToggleEvent onPointerClick = new ToggleEvent();

		protected override void Awake()
		{

			base.Awake();

			this.onValueChanged.AddListener(delegate
			{

				SetRadioColor();

			});

		}

		void SetRadioColor()
		{

			if (outline == null) outline = transform.Find("Background/Outline").GetComponent<Image>();

			if (dot == null) dot = transform.Find("Background/Dot").GetComponent<Image>();

			outline.CrossFadeColor(this.isOn ? accentColor : outlineColor, 0f, true, true);

			dot.CrossFadeColor(this.isOn ? accentColor : new Color(1f, 1f, 1f, 0), 0f, true, true);
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);

			onPointerClick.Invoke(base.isOn);

		}

		public void setColors(Color a, Color b)
		{

			accentColor = a;

			outlineColor = b;

			SetRadioColor();
		}


	}

