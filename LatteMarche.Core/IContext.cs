using System;
namespace LatteMarche.Core
{
	/// <summary>
	///  Astrazione del contesto database
	/// </summary>
	public interface IContext
	{

		/// <summary>
		/// Commit delle modifiche pendenti
		/// </summary>
		/// <returns></returns>
		int SaveChanges();

	}
}
