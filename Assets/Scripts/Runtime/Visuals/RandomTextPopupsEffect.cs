using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Drepanoid.Drivers;
using crass;

namespace Drepanoid
{
    using Random = UnityEngine.Random;

    public class RandomTextPopupsEffect : MonoBehaviour
    {
        [Serializable]
        public class Popup
        {
            public string Text;
            public Vector2 VisibilityTimeRange;
        }

        [Min(1)]
        public int MaxVisiblePopups = 1;
        public Vector2 WaitTimeBetweenPopupsRange;
        public Vector2Int SpawnAreaLowerBounds, SpawnAreaUpperBounds;
        public BagRandomizer<Popup> Popups;

        public TilesetFont Font;
        public SetTextOptions LoadOptions;
        public DeleteTextOptions DeleteOptions;

        readonly List<Tuple<Vector2Int, Vector2Int>> regionsOccupiedByPopups = new List<Tuple<Vector2Int, Vector2Int>>();

        IEnumerator Start ()
        {
            foreach (var popup in Popups.Items)
            {
                popup.Text.Trim('\n'); // so region code works correctly
            }

            while (true)
            {
                yield return new WaitForSeconds(RandomExtra.Range(WaitTimeBetweenPopupsRange));
                if (regionsOccupiedByPopups.Count < MaxVisiblePopups) StartCoroutine(popupRoutine());
            }
        }

        IEnumerator popupRoutine ()
        {
            Popup popup = Popups.GetNext();

            Vector2Int startingPosition;
            do
            {
                startingPosition = new Vector2Int
                (
                    Random.Range(SpawnAreaLowerBounds.x, SpawnAreaUpperBounds.x - popup.Text.Length),
                    Random.Range(SpawnAreaLowerBounds.y, SpawnAreaUpperBounds.y)
                );
                yield return null; // so we don't infinitely lock if there aren't any possible positions
            }
            while (regionsOccupiedByPopups.Any(r => popupWouldIntersectRegion(popup, startingPosition, r)));

            yield return new WaitWhile(() => regionsOccupiedByPopups.Count >= MaxVisiblePopups); // in case the maximum was reached while we were searching

            Tuple<Vector2Int, Vector2Int> popupRegion = new Tuple<Vector2Int, Vector2Int>(startingPosition, startingPosition + Vector2Int.right * popup.Text.Length);
            regionsOccupiedByPopups.Add(popupRegion);

            var setTextArguments = new SetTextArguments(popup.Text, Font, startingPosition);
            yield return Driver.Text.SetText(setTextArguments, LoadOptions);

            yield return new WaitForSeconds(RandomExtra.Range(popup.VisibilityTimeRange));

            var deleteTextArguments = setTextArguments.DeleteArguments();
            yield return Driver.Text.Delete(deleteTextArguments, DeleteOptions);

            regionsOccupiedByPopups.Remove(popupRegion);
        }

        bool popupWouldIntersectRegion (Popup popup, Vector2Int proposedStartingPosition, Tuple<Vector2Int, Vector2Int> region)
        {
            Vector2Int
                regionStart = region.Item1,
                regionEnd = region.Item2,
                popupStart = proposedStartingPosition,
                popupEnd = proposedStartingPosition + Vector2Int.right * popup.Text.Length;

            return
                popupStart.y == regionStart.y &&
                (
                    (popupStart.x >= regionStart.x && popupStart.x <= regionEnd.x) ||
                    (popupEnd.x >= regionStart.x && popupEnd.x <= regionEnd.x)
                );
        }
    }
}
