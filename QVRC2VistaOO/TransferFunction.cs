using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qvrc2VistaOO
{

    public class TransferFunction //: ReactiveObject
    {
       // [Reactive]
        public string Name { get; set; }

        //[Reactive]
        public ObservableCollection<TfPoint> Points { get; set; } = new ObservableCollection<TfPoint>();

        public static TransferFunction[] Presets => new[] { CTBones };

        public static TransferFunction Bones = new TransferFunction()
        {
            Name = "Bones",

            Points = new ObservableCollection<TfPoint>(new[]
                {
                    new TfPoint() { Intensity = 300 , Opacity = 0.0f, JetValue = new Vector3(0.0f, 0.0f, 0.0f) },
                    new TfPoint() { Intensity = 350 , Opacity = 0.75f, JetValue = new Vector3(0.9f, 0.9f, 0.8f) },
                    new TfPoint() { Intensity = 444, Opacity = 0.8f, JetValue = new Vector3(1f, 0.9f, 0.9f) },
                })
        };

        public static TransferFunction CTBones = new TransferFunction()
        {
            Name = "CT Bones",
            Points = new ObservableCollection<TfPoint>(new[]
                {
                    new TfPoint() { Intensity = 1500, Opacity = 0.0f, JetValue = new Vector3(0.25f, 0.0f, 0.0f) },
                    new TfPoint() { Intensity = 1540, Opacity = 0.7f, JetValue = new Vector3(0.5f, 0.25f, 0.0f) },
                    new TfPoint() { Intensity = 1600, Opacity = 1f, JetValue = new Vector3(1f, 1f, 0.9f) },
                })
        };

        public static TransferFunction CTBonesSkin = new TransferFunction()
        {
            Name = "CT Bones + Skin",
            Points = new ObservableCollection<TfPoint>(new[]
        {
                    new TfPoint() { Intensity = -582, Opacity = 0.0f, JetValue = new Vector3(0.727897f, 0.9610279f, 0.8116703f) },
                    new TfPoint() { Intensity = -449, Opacity = 0.7530864f, JetValue = new Vector3(0.9304483f, 0.1824397f, 0.4997101f) },
                    new TfPoint() { Intensity = -218, Opacity = 0.0f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0.9960938f) },
                    new TfPoint() { Intensity = 91, Opacity = 0f, JetValue = new Vector3(0.5f, 0f, 0f) },
                    new TfPoint() { Intensity = 112, Opacity = 0.3827161f, JetValue = new Vector3(0.9960938f, 0f, 0f) },
                    new TfPoint() { Intensity = 183, Opacity = 0.691358f, JetValue = new Vector3(0.25f, 0f, 0f) },
                    new TfPoint() { Intensity = 262, Opacity = 1f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0.9960938f) },
                })
        };

        public static TransferFunction CTFattissue = new TransferFunction()
        {
            Name = "CT Fat tissue",
            Points = new ObservableCollection<TfPoint>(new[]
            {
                    new TfPoint() { Intensity = -207, Opacity = 0.0f, JetValue = new Vector3(0.0f, 0.0f, 0.0f) },
                    new TfPoint() { Intensity = -189, Opacity = 0.6172839f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0f) },
                    new TfPoint() { Intensity = -106, Opacity = 0.6666667f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0.0f) },
                    new TfPoint() { Intensity = -71, Opacity = 0.0f, JetValue = new Vector3(0.0f, 0.0f, 0.0f) },
            })
        };

        public static TransferFunction CTLungs = new TransferFunction()
        {
            Name = "CT Lungs",
            Points = new ObservableCollection<TfPoint>(new[]
            {
                    new TfPoint() { Intensity = -582, Opacity = 0.0f, JetValue = new Vector3(0.0f, 0.0f, 0.9960938f) },
                    new TfPoint() { Intensity = -449, Opacity = 0.7530864f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0.9960938f) },
                    new TfPoint() { Intensity = -218, Opacity = 0.0f, JetValue = new Vector3(0.0f, 0.0f, 0f) },
            })
        };

        public static TransferFunction CTMuscles = new TransferFunction()
        {
            Name = "CT Muscles",
            Points = new ObservableCollection<TfPoint>(new[]
            {
                    new TfPoint() { Intensity = 366, Opacity = 1.0f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0.9960938f) },
                    new TfPoint() { Intensity = -71, Opacity = 0.0f, JetValue = new Vector3(0.5f, 0.0f, 0.0f) },
                    new TfPoint() { Intensity = 112, Opacity = 0.4814815f, JetValue = new Vector3(0.0f, 0.0f, 0f) },
                    new TfPoint() { Intensity = 197, Opacity = 0.7037037f, JetValue = new Vector3(0.9960938f, 0.5f, 0.25f) },
                    new TfPoint() { Intensity = 11, Opacity = 0.2962963f, JetValue = new Vector3(0.9960938f, 0.0f, 0.0f) },
            })
        };

        public static TransferFunction CTSkin = new TransferFunction()
        {
            Name = "CT Skin",
            Points = new ObservableCollection<TfPoint>(new[]
            {
                    new TfPoint() { Intensity = -310, Opacity = 1.0f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0.9960938f) },
                    new TfPoint() { Intensity = -582, Opacity = 0.0f, JetValue = new Vector3(0.5f, 0.5f, 0.0f) },
            })
        };

        public static TransferFunction CTVessels = new TransferFunction()
        {
            Name = "CT Vessels",
            Points = new ObservableCollection<TfPoint>(new[]
            {
                    new TfPoint() { Intensity = 262, Opacity = 1.0f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0.9960938f) },
                    new TfPoint() { Intensity = 67, Opacity = 0.0f, JetValue = new Vector3(0.5f, 0.5f, 0.0f) },
                    new TfPoint() { Intensity = 112, Opacity = 0.3827161f, JetValue = new Vector3(0.9960938f, 0.0f, 0.0f) },
                    new TfPoint() { Intensity = 183, Opacity = 0.691358f, JetValue = new Vector3(0.25f, 0.0f, 0.0f) },
            })
        };

        public static TransferFunction CTVessels2 = new TransferFunction()
        {
            Name = "CT Vessels2",
            Points = new ObservableCollection<TfPoint>(new[]
            {
                    new TfPoint() { Intensity = 249, Opacity = 1.0f, JetValue = new Vector3(0.9960938f, 0.9960938f, 0.9960938f) },
                    new TfPoint() { Intensity = 161, Opacity = 0.7901235f, JetValue = new Vector3(0.9960938f, 0.5f, 0.25f) },
                    new TfPoint() { Intensity = 111, Opacity = 0.5802469f, JetValue = new Vector3(0.9960938f, 0.0f, 0.0f) },
                    new TfPoint() { Intensity = 71, Opacity = 0.0f, JetValue = new Vector3(0.25f, 0.0f, 0.0f) },
                    new TfPoint() { Intensity = 83, Opacity = 0.2962963f, JetValue = new Vector3(0.5f, 0.0f, 0.0f) },
            })
        };
    }

    [Serializable]
    public class TfPoint
    {
        public Vector3 JetValue { get; set; }

        public int Intensity { get; set; }

        public float Opacity { get; set; }

    }
}
