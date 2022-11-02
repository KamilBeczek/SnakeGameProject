using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameProject
{
    public class ObjectPosition
    {
        public int PostionRow { get; }
        public int PostionColumn { get; }

        public ObjectPosition(int row, int column)
        {
            PostionRow = row;
            PostionColumn = column;
        }

        public ObjectPosition Translate(SnakeDirection dir)
        {
            ObjectPosition afterMove = new ObjectPosition(PostionRow + dir.RowOffset, PostionColumn + dir.ColOffset);
            return afterMove;
        }

    }
}
