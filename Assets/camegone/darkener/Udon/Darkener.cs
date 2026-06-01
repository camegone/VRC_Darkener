
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;

namespace camegone.Darkener
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Darkener : UdonSharpBehaviour
    {
        [FieldChangeCallback(nameof(Brightness))] [SerializeField] private float _brightness = 1.0f;
        public float Brightness
        {
            get { return _brightness; }
            set { _brightness = value; UpdateColor(); }
        }
        [FieldChangeCallback(nameof(Tint))] [SerializeField] private Color _tint = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public Color Tint
        {
            get { return _tint; }
            set { _tint = value; UpdateColor(); }
        }
        [SerializeField] private Slider _bright;
        [SerializeField] private Slider _red;
        [SerializeField] private Slider _green;
        [SerializeField] private Slider _blue;
        [SerializeField] private Toggle _cameraFxToggle;

        [SerializeField] private MeshRenderer _darkener;

        //[SerializeField] private GlobalKeyword _keyword;

        private readonly Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        private MaterialPropertyBlock _propBlock = null;
        private MaterialPropertyBlock PropBlock
        {
            // create new property block if reference first time
            get { return _propBlock ?? (_propBlock = new MaterialPropertyBlock());}
        }

        void Start()
        {
            if (_darkener == null)
                _darkener = this.GetComponent<MeshRenderer>();
            if (_bright == null)
                _bright = this.gameObject.transform.parent.Find("Canvas").Find("BrightSlider").gameObject.GetComponent<Slider>();

            //Shader.SetKeyword("TEST", true);
            UpdateColor();
        }

        public void UpdateColor() {
            UpdateRenderer();

            // use property block to optimize
            PropBlock.SetColor("_UdonDarkenerColor", _tint * VecBright(_brightness));
            if (_cameraFxToggle)
                PropBlock.SetFloat("_IsShownInNonUserCamera", _cameraFxToggle.isOn ? 1.0f : 0.0f);
            _darkener.SetPropertyBlock(PropBlock);
            // _darkener.material.SetColor("_UdonDarkenerColor", _tint * VecBright(_brightness)); <- old method
        }

        Color VecBright(float b)
        {
            return new Color(b, b, b, 1.0f);
        }

        void UpdateRenderer()
        {
            if (_brightness >= 1.0f && _tint == white)
                _darkener.enabled = false;
            else
                _darkener.enabled = true;
        }

        float GetSliderVal(Slider slider)
        {
            return slider.value;
        }

        public void OnBrightnessChanged()
        {
            _brightness = GetSliderVal(_bright);
            UpdateColor();
        }

        public void OnRedChanged()
        {
            _tint.r = GetSliderVal(_red);
            UpdateColor();
        }

        public void OnGreenChanged()
        {
            _tint.g = GetSliderVal(_green);
            UpdateColor();
        }

        public void OnBlueChanged()
        {
            _tint.b = GetSliderVal(_blue);
            UpdateColor();
        }

        public void OnCameraFxToggled()
        {
            UpdateColor();
        }

    }
}