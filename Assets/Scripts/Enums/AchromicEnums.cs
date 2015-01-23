namespace Assets.Scripts.Enums
{
	//all the color elements
	public enum ColorElement
	{
		Red,        // r
		Orange,     // rr+g
		Yellow,     // r+g
		Chartreuse, // r+gg
		Green,      // g
		Spring,     // gg+b
		Cyan,       // g+b
		Azure,      // g+bb
		Blue,       // b
		Violet,     // r+bb
		Magenta,    // r+b
		Rose,       // rr+b

		// none
		White,

		// all
		Black,      // r+b+g

		NumTypes
	}

	//state of the player
	public enum PlayerState
	{
		Attack,
		Defend,
		Jump,
		Achromic,

		NumTypes
	}

	//state of the game
	public enum GameState
	{
		Sponsor,
		Title,
		Settings,
		Game,
		Overworld,
		Pause,
		EndLevel,
		Credits,

		NumTypes
	}

	//state of a level
	public enum LevelStates
	{
		Incomplete,
		InProgress,
		Complete,

		NumTypes
	}
}