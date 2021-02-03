using UnityEngine;

namespace DefaultNamespace
{
    public class Physics
    {
        private Settings _settings;

        private Vector3Int[] _dir = new Vector3Int[]
        {
            new Vector3Int(-1, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(1, -1, 0),
            new Vector3Int(0, 0, 0),
        };

        public Physics()
        {
            _settings = Bootstrap.Instance.Settings;
        }

        public bool Collide(Vector3 position, out Vector3 outPosition)
        {
            var pos = _settings.CollidersMap.WorldToCell(position);

            var tl = _settings.CollidersMap.GetTile(pos);

            if (tl == null)
            {
                outPosition = position;
                return false;
            }

            var index = 0;
            for (; index < _dir.Length; index++)
            {
                tl = _settings.CollidersMap.GetTile(pos + _dir[index]);
                
                if (tl == null)
                    break;
            }

            outPosition = _settings.CollidersMap.GetCellCenterWorld(pos + _dir[index]);
            return true;
        }

        public bool Move(Vector3 position, Vector3Int dir, out Vector3 outPosition)
        {
            var pos = _settings.CollidersMap.WorldToCell(position);
            var newPosition = pos + dir;

            var tl = _settings.CollidersMap.GetTile(newPosition);

            if (tl == null)
            {
                outPosition = _settings.CollidersMap.GetCellCenterWorld(newPosition);
                return true;
            }

            outPosition = position;

            return false;
        }

        public Vector3 SetPosition(Vector3 position)
        {
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);
            position.z = Mathf.Round(position.z);

            return position;
        }
    }
}