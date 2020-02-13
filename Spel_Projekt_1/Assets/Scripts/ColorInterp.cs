using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorInterp : MonoBehaviour {
	public class ColorComponent {
		protected Graphic graphic;
		protected Light light;
		protected SpriteRenderer sprite;

		public Color color {
			get {
				if (sprite) {
					return sprite.color;
				} else if (graphic) {
					return graphic.color;
				} else if (light) {
					return light.color;
				}

				return Color.clear;
			}
			set {
				if (sprite) {
					sprite.color = value;
				} else if (graphic) {
					graphic.color = value;
				} else if (light) {
					light.color = value;
				}
			}
		}

		public Component component {
			get {
				if (sprite) {
					return sprite;
				} else if (graphic) {
					return graphic;
				} else if (light) {
					return light;
				}
				return null;
			}
			set {
				sprite = value as SpriteRenderer;
				graphic = value as Graphic;
				light = value as Light;
			}
		}
	}

	public enum PlayMode {
		PlayOnce,
		Loop,
		SingleBounce,
		LoopBounce,
	}
	public enum InterpMode {
		Linear,
		Polynomial,
	}
	public enum ColorMode {
		RGB,
		HSV,
	}

	[SerializeField]
	protected Component graphic;
	[SerializeField]
	protected bool playOnAwake = true;
	[SerializeField]
	protected Color startColor;
	[SerializeField]
	protected Color targetColor;
	[SerializeField]
	protected PlayMode playMode;
	[SerializeField]
	protected InterpMode interpMode;
	[SerializeField]
	protected ColorMode colorMode;
	[SerializeField]
	protected float speed = 1;

	protected ColorComponent component = new ColorComponent();

	protected Color _c0;
	protected Color _c1;
	protected PlayMode _playMode;
	protected InterpMode _interp;
	protected ColorMode _colorMode;
	protected float _speed;

	protected bool _playing = false;
	protected float _progress = 0;
	protected bool _bouncing = false;

    // Start is called before the first frame update
    void Start()
    {
		if (graphic == null) {
			graphic = transform.GetComponentInChildren<SpriteRenderer>();
		}
		if (graphic == null) {
			graphic = transform.GetComponentInChildren<Graphic>();
		}
		if (graphic == null) {
			graphic = transform.GetComponentInChildren<Light>();
		}

		//Debug.Log(gameObject.name);
		component.component = graphic;

		if (playOnAwake) {
			Play();
		}
    }

	private void OnValidate() {
		if (!(graphic is SpriteRenderer) && !(graphic is Graphic) && !(graphic is Light)) {
			graphic = null;
		}

		component.component = graphic;
	}

	// Update is called once per frame
	void Update()
    {
		if (_playing) {
			if (_bouncing) {
				_progress -= _speed * Time.deltaTime;

				if (_progress < 0) {
					_bouncing = false;
					switch (_playMode) {
						case PlayMode.SingleBounce:
							_progress = 0;
							_playing = false;
							break;
						case PlayMode.LoopBounce:
							_progress = 0;
							break;
					}
				}
			} else {
				_progress += _speed * Time.deltaTime;

				if (_progress > 1) {
					switch (_playMode) {
						case PlayMode.PlayOnce:
							_progress = 1;
							_playing = false;
							break;
						case PlayMode.Loop:
							_progress -= 1;
							break;
						case PlayMode.SingleBounce:
							_progress = 1;
							_bouncing = true;
							break;
						case PlayMode.LoopBounce:
							_progress = 1;
							_bouncing = true;
							break;
					}
				}
			}

			float t = _progress;

			switch (_interp) {
				case InterpMode.Linear:
					break;
				case InterpMode.Polynomial:
					t = 3 * _progress * _progress - 2 * _progress * _progress * _progress;
					break;
			}

			switch (_colorMode) {
				case ColorMode.RGB:
					component.color = Color.Lerp(_c0, _c1, t);
					break;
				case ColorMode.HSV:
					component.color = LerpHSV(_c0, _c1, t);
					break;
			}
		}
	}

	public Color LerpHSV(Color c0, Color c1, float t) {
		float h0, s0, v0, h1, s1, v1;
		Color.RGBToHSV(c0, out h0, out s0, out v0);
		Color.RGBToHSV(c1, out h1, out s1, out v1);

		var dh = h1 - h0;
		float h = h0 + dh * t;
		if (dh > 0.5f) {
			h = h0 - (1 - dh) * t;
			if (h < 0) {
				h++;
			} else if (h > 1) {
				h--;
			}
		} else if (dh < -0.5f) {
			h = h0 + (dh + 1) * t;
			if (h > 1) {
				h--;
			}
		}

		var s = Mathf.Lerp(s0, s1, t);
		var v = Mathf.Lerp(v0, v1, t);

		return Color.HSVToRGB(h, s, v);
	}

	public void Play(Color startColor,Color targetColor, float speed, PlayMode playMode,InterpMode interpMode, ColorMode colorMode) {
		if (component.component != null) {
			_c0 = startColor;
			_c1 = targetColor;
			_speed = speed;
			_playMode = playMode;
			_interp = interpMode;
			_colorMode = colorMode;

			_progress = 0;
			component.color = startColor;
			_bouncing = false;
			_playing = true;
		}
	}

	public void Play(Color targetColor, float speed, PlayMode playMode, InterpMode interpMode, ColorMode colorMode) {
		Play(component.color, targetColor, speed, playMode, interpMode, colorMode);
	}

	public void Play(Color targetColor, float speed) {
		Play(component.color, targetColor, speed, playMode, interpMode, colorMode);
	}

	public void Play(Color startColor, Color targetColor, float speed) {
		Play(startColor, targetColor, speed, playMode, interpMode, colorMode);
	}

	public void Play() {
		Play(startColor, targetColor, speed, playMode, interpMode, colorMode);
	}
}
