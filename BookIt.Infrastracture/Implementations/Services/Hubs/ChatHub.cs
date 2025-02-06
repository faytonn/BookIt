//using BookIt.Application.DTOs.ChatDTO;
//using Microsoft.AspNetCore.SignalR;

//namespace BookIt.Persistence.Services.Hubs;

//public class ChatHub : Hub
//{
//    public static List<ConnectionDTO> Connections = [];
//    private readonly ICookieService _cookieService;

//    public ChatHub(ICookieService cookieService)
//    {
//        _cookieService = cookieService;
//    }

//    public override Task OnConnectedAsync()
//    {
//        AddConnectionIds();

//        return base.OnConnectedAsync();
//    }


//    public override Task OnDisconnectedAsync(Exception? exception)
//    {
//        //var userId = _cookieService.GetUserId();

//        //Connections.RemoveAll(x => x.UserId == userId);

//        return base.OnDisconnectedAsync(exception);
//    }

//    public async Task SendMessage(string connectionId, string message)
//    {
//        await Clients.Client(connectionId).SendAsync("ReceiveChatMessage", message);
//    }
//    private void AddConnectionIds()
//    {
//        //var userId = _cookieService.GetUserId();

//        //if (userId == null)
//        //    throw new UnAuthorizedException();

//        //var connection = Connections.FirstOrDefault(x => x.UserId == userId);

//        //if (connection == null)
//        {
//            ConnectionDTO dto = new()
//            {
//                //UserId = userId,
//                ConnectionIds = [Context.ConnectionId]
//            };
//            Connections.Add(dto);
//        }
//        else
//        {
//            //connection.ConnectionIds.Add(Context.ConnectionId);
//        }
//    }
//}
