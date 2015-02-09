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
		idle = 0, move, jump, jump2, inAir, attack1, attack2, attack3, movingAttack, inAirAttack, parry, block, crouch, hit, dead, NumTypes
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
		Lose,
		Win,
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