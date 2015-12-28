namespace CountryTrash
{
	internal class GrasTile : Tile
	{
		public GrasTile(Tile other) 
			: base(other)
		{
			this.Model = "/Models/tile";
		}

		public override void Interact(Player player)
		{
			player.AddTask(new ChangeTileTask(new DirtTile(this.Map, this.X, this.Z)));
		}
	}
}