namespace MojoCase.Utilities
{
    [System.Serializable]
    public class FloatReference
    {
        public bool UseConstant;
        public float ConstantValue;
        public FloatVariable Variable;

        public float Value
        {
            get => UseConstant ? ConstantValue : Variable.Value;
            set => Variable.Value = value;
        }
    }
}