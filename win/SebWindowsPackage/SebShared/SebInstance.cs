namespace SebShared
{
	public static class SebInstance
	{
		public static SebSettings Settings { get; set; }

		static SebInstance()
		{
			Settings = new SebSettings();
		}
	}
}