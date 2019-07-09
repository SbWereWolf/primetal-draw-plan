namespace draw_plan
{
    class MinMax
    {
        private int _maxValue;
        private int _minValue;

        public MinMax(int value)
        {
            this._minValue = value;
            this._maxValue = value;
        }

        public MinMax Adopt(int value)
        {
            if (value > _maxValue)
            {
                _maxValue = value;
            }

            if (value < _minValue)
            {
                _minValue = value;
            }

            return this;

        }

        public int GetDifference()
        {
            return _maxValue - _minValue;
        }
    }
}
