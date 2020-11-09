using System.Threading.Tasks;

namespace AppName.Core
{
	public interface ITriggerAction
	{
		Task Execute();
	}
}
