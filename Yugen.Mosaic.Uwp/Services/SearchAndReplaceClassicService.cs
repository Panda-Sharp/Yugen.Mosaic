﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yugen.Mosaic.Uwp.Helpers;
using Yugen.Mosaic.Uwp.Models;
using Yugen.Toolkit.Standard.Services;

namespace Yugen.Mosaic.Uwp.Services
{
    public class SearchAndReplaceClassicService : SearchAndReplaceService
    {
        public SearchAndReplaceClassicService(IProgressService progressService) : base(progressService)
        {
        }

        public override Image<Rgba32> SearchAndReplace()
        {
            _progressService.Reset();

            int max = _tX * _tY;

            Parallel.For(0, _tX * _tY, xy =>
            {
                int y = xy / _tX;
                int x = xy % _tX;

                int difference = 1000;
                List<TileFound> tileFoundList = new List<TileFound>();

                // Search for a tile with a similar color
                foreach (var tile in _tileImageList)
                {
                    var newDifference = ColorHelper.GetDifference(_avgsMaster[x, y], tile.AverageColor);
                    if (newDifference <= (difference + 5))
                    {
                        tileFoundList.Add(new TileFound(tile, newDifference));
                        difference = newDifference;
                    }
                }

                // Choose a random tile from tileFoundList with a threshold +/- 5 the best match
                var threshold = tileFoundList.Min(t1 => t1.Difference) + 5;
                var r = new Random();
                var tileFound = tileFoundList.Where(t2 => t2.Difference <= threshold)
                    .OrderBy(a => r.Next())
                        .First().Tile;

                // Apply found tile to section
                ApplyTileFound(x, y, tileFound.ResizedImage);

                _progressService.IncrementProgress(max, 66, 100);
            });

            return _outputImage;
        }
    }
}