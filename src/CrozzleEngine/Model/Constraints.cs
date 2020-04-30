using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrozzleEngine.Model
{
    /// <summary>
    /// Constraints
    /// </summary>
    public static class Constraints
    {
        private const int CELL_NEIGHBORS = 3;
        private const int EASY_CONSTRAINT_INTERSECT = 2;
        private const int MEDIUM_CONTRAINT_INTERSECT = 3;
        private const int COMMON_MIN_CONTRAINT_INTERSECT = 1;

        private static bool AllFilled(List<Cell> cells)
        {
            return cells.Count >= CELL_NEIGHBORS && cells.All(cell => !cell.CharacterIsEmpty());
        }

        /// <summary>
        /// spaces mast be
        /// </summary>
        /// <param name="cells">cells</param>
        /// <param name="startPosition">start cell of word</param>
        /// <param name="endPosition">end cell of word</param>
        /// <param name="orientationIndex">index of orientation, to determine cell correctly</param>
        /// <param name="orientation">word orientation <see cref="WordOrientation"/></param>
        /// <returns>true - spaces exists, otherwise - not</returns>
        public static bool DelimetedBySpaces(this List<Cell> cells, int startPosition, int endPosition, int orientationIndex, WordOrientation orientation)
        {
            Cell first, last;
            bool firstIsValid = false, lastIsValid = false, enoughSpace = true;

            foreach (var cell in cells)
            {
                if (cell.CharacterIsEmpty() || !cell.EnoughSpace)
                {
                    continue;
                }

                var neighbors = cells.Where(c => (c.ColumnIndex == cell.GetNeighborColumn(NeighborOrientation.Horizontal) && c.RowIndex == cell.GetNeighborRow(NeighborOrientation.Horizontal))
                                 || (c.ColumnIndex == cell.GetNeighborColumn(NeighborOrientation.Vertical) && c.RowIndex == cell.GetNeighborRow(NeighborOrientation.Vertical))
                                 || (c.ColumnIndex == cell.GetNeighborColumn(NeighborOrientation.Diagonal) && c.RowIndex == cell.GetNeighborRow(NeighborOrientation.Diagonal)))
                                .ToList();

                if (AllFilled(neighbors))
                {
                    enoughSpace = cell.EnoughSpace = false;
                }
            }

            if (orientation == WordOrientation.Horizontal)
            {
                first = cells.FirstOrDefault(cell => cell.ColumnIndex == startPosition - 1 && cell.RowIndex == orientationIndex);
                last = cells.FirstOrDefault(cell => cell.ColumnIndex == endPosition && cell.RowIndex == orientationIndex);

                if (first == null && last == null)
                {
                    return enoughSpace;
                }

                firstIsValid = first?.CharacterIsEmpty() ?? true;

                lastIsValid = last?.CharacterIsEmpty() ?? true;

            }

            if (orientation == WordOrientation.Vertical)
            {
                first = cells.FirstOrDefault(cell => cell.RowIndex == startPosition - 1 && cell.ColumnIndex == orientationIndex);
                last = cells.FirstOrDefault(cell => cell.RowIndex == endPosition && cell.ColumnIndex == orientationIndex);

                if (first == null && last == null)
                {
                    return enoughSpace;
                }

                firstIsValid = first?.CharacterIsEmpty() ?? true;

                lastIsValid = last?.CharacterIsEmpty() ?? true;
            }

            return enoughSpace && firstIsValid && lastIsValid;
        }

        /// <summary>
        /// Word left to right, top to bottom notation is only correct
        /// </summary>
        /// <param name="startPosition">start position</param>
        /// <param name="endPosition">end position</param>
        /// <returns>true - word is correct, otherwise it's not</returns>
        public static bool WordOrientationIsCorrect(int startPosition, int endPosition)
        {
            return endPosition > startPosition;
        }

        /// <summary>
        /// Intersecting difficulty level constraints
        /// </summary>
        /// <param name="cells">cells</param>
        /// <param name="level">difficult</param>
        /// <returns>All rith if it's true, otherwise - false</returns>
        public static bool IntersectingConstarint(this List<Cell> cells, Difficult level)
        {
            var intersectIsOk = false;
            switch (level)
            {
                case Difficult.Easy:
                    intersectIsOk = cells.Any(cell => !(cell.Words.Count > COMMON_MIN_CONTRAINT_INTERSECT && cell.Words.Count <= EASY_CONSTRAINT_INTERSECT));
                    break;
                case Difficult.Medium:
                    intersectIsOk = cells.Any(cell => !(cell.Words.Count > COMMON_MIN_CONTRAINT_INTERSECT && cell.Words.Count <= MEDIUM_CONTRAINT_INTERSECT));
                    break;
                case Difficult.Hard:
                    intersectIsOk = cells.Any(cell => !(cell.Words.Count > COMMON_MIN_CONTRAINT_INTERSECT));
                    break;
            }
            return intersectIsOk;
        }

    }
}
