using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class BulletFactory : ViewController
	{
		public static BulletFactory Default;

        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;
        }
        void Start()
		{

		}
	}
}
