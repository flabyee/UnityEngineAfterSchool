using UnityEngine;

namespace UnityEditor.Tilemaps
{
    [CreateAssetMenu(fileName = "Afterschool Flip brush", menuName = "Brushes/Afterschool Flip brush", order = 361)]
    [CustomGridBrush(true, false, false, "Afterschool Flip brush")]
    
    public class AfterschoolFlipBrush : GridBrush
    {
        public bool m_FlipX;
        public bool m_FlipY;

        public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pickStart)
        {
            if (m_FlipX)
                base.Flip(FlipAxis.X, gridLayout.cellLayout);
            if (m_FlipY)
                base.Flip(FlipAxis.Y, gridLayout.cellLayout);

            base.Pick(gridLayout, brushTarget, position, pickStart);

            if (m_FlipX)
                base.Flip(FlipAxis.X, gridLayout.cellLayout);
            if (m_FlipY)
                base.Flip(FlipAxis.Y, gridLayout.cellLayout);
        }

        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            base.Paint(gridLayout, brushTarget, position);
        }
    }
}