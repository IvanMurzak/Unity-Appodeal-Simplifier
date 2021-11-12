using UnityEngine;

namespace AppodealSimplifier
{
	public partial class AppodealSimplifierStarter : MonoBehaviour
	{
		[SerializeField] bool initializeOnStart = true;

		protected virtual void Start()
		{
			if (initializeOnStart) Initialize();
		}

		public void Initialize()
		{
			AppodealSimplifier.Initialize();
		}
	}
}