using Mangopollo;
using Mangopollo.Tiles;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Linq;
using workout7.Resources;

namespace workout7.Helpers
{
    static class TileManager
    {
        public static void UpdatePrimaryTile(int count)
        {
            // only if I can use WP8 tiles - update the first tile
            if (Utils.CanUseLiveTiles)
            {
                var tileId = ShellTile.ActiveTiles.FirstOrDefault();
                if (tileId != null)
                {
                    var tileData = new FlipTileData();
                    tileData.Title = AppResources.ApplicationTitle;
                    // tileData.Count = count;
                    tileData.BackgroundImage = new Uri("/Images/tile.medium.png", UriKind.Relative);
                    tileData.WideBackgroundImage = new Uri("/Images/tile.wide.png", UriKind.Relative);

                    if (count > 0)
                    {
                        tileData.Count = count;
                    }
#if DEBUG
                    Debug.WriteLine("Activating live tile: " + Mangopollo.Utils.CanUseLiveTiles);
#endif
                    tileId.Update(tileData);
                }
            }
        }
    }
}
