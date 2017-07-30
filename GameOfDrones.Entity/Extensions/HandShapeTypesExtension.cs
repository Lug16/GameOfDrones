using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones.Entity.Extensions
{
    public static class HandShapeTypesExtension
    {
        public static Tuple<bool,bool> GetResult(this HandShapeTypes current, HandShapeTypes previous)
        {
            var won = false;
            var lose = false;

            switch (current)
            {
                case HandShapeTypes.Paper:
                    won = previous == HandShapeTypes.Rock;
                    lose = previous == HandShapeTypes.Scissors;
                    break;
                case HandShapeTypes.Rock:
                    won = previous == HandShapeTypes.Scissors;
                    lose = previous == HandShapeTypes.Paper;
                    break;
                case HandShapeTypes.Scissors:
                    won = previous == HandShapeTypes.Paper;
                    lose = previous == HandShapeTypes.Rock;
                    break;
                default:
                    break;
            }

            return Tuple.Create(won, lose);
        }
    }
}
