namespace Form1
{
	public struct DVar
	{
		public string name;

		public string description;

		public bool isDecimal;

		public decimal decimalMin;

		public decimal decimalMax;

		public decimal decimalIncrement;

		public DVar(string name, string description)
		{
			this.name = name;
			this.description = description;
			isDecimal = false;
			decimalMin = 0m;
			decimalMax = 0m;
			decimalIncrement = 0m;
		}

		public DVar(string name, string description, decimal decimalMin, decimal decimalMax, decimal decimalIncrement)
		{
			this.name = name;
			this.description = description;
			isDecimal = true;
			this.decimalMin = decimalMin;
			this.decimalMax = decimalMax;
			this.decimalIncrement = decimalIncrement;
		}

		public DVar(string name, string description, decimal decimalMin, decimal decimalMax)
		{
			this.name = name;
			this.description = description;
			isDecimal = true;
			this.decimalMin = decimalMin;
			this.decimalMax = decimalMax;
			decimalIncrement = 1m;
		}
	}
}
