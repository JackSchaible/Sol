public abstract class IShipStats 
{
	public float ForwardThrustMax { get; protected set; }
	public float RotationalThrustMax { get; protected set; }
	public float Mass { get; protected set; }

	public IShipStats ()
	{
		
	}
}