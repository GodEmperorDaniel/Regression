using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorInterp : MonoBehaviour
    {
        public enum PlayMode
        {
            PlayOnce,
            Loop,
            SingleBounce,
            LoopBounce,
        }
        public enum InterpMode
        {
            Linear,
            Polynomial,
        }

        [SerializeField]
        protected Graphic graphic;
        [SerializeField]
        protected bool playOnAwake;
        [SerializeField]
        protected Color startColor;
        [SerializeField]
        protected Color targetColor;
        [SerializeField]
        protected PlayMode playMode;
        [SerializeField]
        protected InterpMode interpMode;
        [SerializeField]
        protected float speed = 1;

        protected Graphic _targetG;
        protected Color _c0;
        protected Color _c1;
        protected PlayMode _playMode;
        protected InterpMode _interp;
        protected float _speed;

        protected bool playing = false;
        protected float progress = 0;
        protected bool bouncing = false;

        // Start is called before the first frame update
        void Start()
        {
            if (graphic == null)
            {
                graphic = transform.GetComponentInChildren<Graphic>();
            }
            if (playOnAwake)
            {
                Play(startColor, targetColor, speed, playMode, interpMode);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (playing)
            {
                if (bouncing)
                {
                    progress -= speed * Time.deltaTime;

                    if (progress < 0)
                    {
                        bouncing = false;
                        switch (playMode)
                        {
                            case PlayMode.SingleBounce:
                                progress = 0;
                                playing = false;
                                break;
                            case PlayMode.LoopBounce:
                                progress = 0;
                                break;
                        }
                    }
                }
                else
                {
                    progress += speed * Time.deltaTime;

                    if (progress > 1)
                    {
                        switch (playMode)
                        {
                            case PlayMode.PlayOnce:
                                progress = 1;
                                playing = false;
                                break;
                            case PlayMode.Loop:
                                progress -= 1;
                                break;
                            case PlayMode.SingleBounce:
                                progress = 1;
                                bouncing = true;
                                break;
                            case PlayMode.LoopBounce:
                                progress = 1;
                                bouncing = true;
                                break;
                        }
                    }
                }

                float t = progress;

                switch (interpMode)
                {
                    case InterpMode.Linear:
                        break;
                    case InterpMode.Polynomial:
                        t = 3 * progress * progress - 2 * progress * progress * progress;
                        break;
                }

                graphic.color = Color.Lerp(startColor, targetColor, t);
            }
        }

        public void Play(Color startColor, Color targetColor, float speed, PlayMode playMode, InterpMode interpMode)
        {
            if (graphic != null)
            {
                _c0 = startColor;
                _c1 = targetColor;
                _speed = speed;
                _playMode = playMode;
                _interp = interpMode;

                progress = 0;
                graphic.color = startColor;
                bouncing = false;
                playing = true;
            }
        }
    }
