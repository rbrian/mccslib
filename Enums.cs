namespace mccslib
{
	public enum DigStatus : byte
	{
		Started = 0,
		Finished = 2,
		Drop = 4
	}

	public enum FaceOffset : byte
	{
		MinusY = 0,
		PlusY = 1,
		MinusZ = 2,
		PlusZ = 3,
		MinusX = 4,
		PlusX = 5
	}

	public enum MovingObject : byte
	{
		Boats = 01,
		Minecart = 10,
		StorageCart = 11, 
		PoweredCart = 12,
		ActivatedTNT = 50,
		Arrow = 60,
		ThrownSnowball = 61,
		ThrownEgg = 62,
		FallingSand = 70,
		FallingGravel = 71,
		FishingFloat = 90
	}

	public enum MobType : byte
	{
		Creeper = 50,
		Skeleton = 51,
		Spider = 52,
		GiantZombie = 53,
		Zombie = 54,
		Slime = 55,
		Ghast = 56,
		ZombiePigman = 57,
		Pig = 90,
		Sheep = 91,
		Cow = 92,
		Hen = 93,
		Squid = 94,
		Wolf = 95,
	}

	public enum WoolColor : byte
	{
		White = 0,
		Orange = 1,
		Magenta = 2,
		LightBlue = 3,
		Yellow = 4,
		Lime = 5,
		Pink = 6,
		Gray = 7,	
		Silver = 8,
		Cyan = 9,
		Purple = 10,
		Blue = 11,
		Brown = 12,
		Green = 13,
		Red = 14,
		Black = 15,
	}
}
