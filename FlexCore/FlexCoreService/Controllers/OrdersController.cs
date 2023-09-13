﻿using EFModels.Models;
using FlexCoreService.Orders;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Collections.Concurrent;

namespace FlexCoreService.Controllers
{
	[EnableCors("AllowAny")]
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly AppDbContext _context;
		public OrdersController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet("GetOrders")]
		public async Task<ActionResult<IEnumerable<OrdersIndexVM>>> GetOrders(string? keyword, int? typeId, DateTime? begintime, DateTime? endtime, int? ostatusId, int? memberId)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return Ok(null);
			}
			if (begintime.HasValue && endtime.HasValue && begintime.Value > endtime.Value)
			{

				ModelState.AddModelError("begintime", "開始時間不能大於結束時間");

				begintime = null;
				endtime = null;
			}
			var orderStatuses = db.order_statuses.AsNoTracking().ToDictionary(os => os.Id, os => os.order_status1);
			var paymethods = db.pay_methods.AsNoTracking().ToDictionary(pd => pd.Id, pd => pd.pay_method1);
			var paystatuses = db.pay_statuses.AsNoTracking().ToDictionary(ps => ps.Id, ps => ps.pay_status1);
			var query = typeId.HasValue
			? _context.orders.Where(o => o.fk_typeId == typeId)
			: _context.orders;
			if (memberId.HasValue)
			{
				query = query.Where(o => o.fk_member_Id == memberId
				);
			}
			if (ostatusId.HasValue)
			{
				query = query.Where(o => o.order_status_Id == ostatusId
				);
			}
			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(o =>
					o.orderItems.Any(oi => oi.product_name.Contains(keyword))
				);
			}
			if (begintime.HasValue && endtime.HasValue)
			{

				query = query.Where(o =>
				o.ordertime >= begintime.Value && o.ordertime <= endtime.Value.AddDays(1)
			);

			}
			query = query.OrderByDescending(o => o.ordertime);

			var result = query.Select(p => new OrdersIndexVM
			{
				Id = p.Id,
				ordertime = p.ordertime,
				fk_member_Id = p.fk_member_Id,
				total_quantity = p.total_quantity,
				order_status_Id = p.order_status_Id,
				order_status = orderStatuses.ContainsKey(p.order_status_Id) ? orderStatuses[p.order_status_Id] : string.Empty,
				pay_method_Id = p.pay_method_Id,
				pay_method = paymethods.ContainsKey(p.pay_method_Id) ? paymethods[p.pay_method_Id] : string.Empty,
				pay_status_Id = p.pay_status_Id,
				pay_status = paystatuses.ContainsKey(p.pay_status_Id) ? paystatuses[p.pay_status_Id] : string.Empty,
				coupon_name = p.coupon_name,
				coupon_discount = p.coupon_discount,
				freight = p.freight,
				cellphone = p.cellphone,
				receipt = p.receipt,
				receiver = p.receiver,
				recipient_address = p.recipient_address,
				order_description = p.order_description,
				close = (bool)p.close,
				close_time = p.close_time,
				total_price = p.total_price,
				fk_typeId = p.fk_typeId,
				biller = p.biller,
				biller_adress = p.bill_address,
				biller_cellphone = p.bill_cellphone,
				orderCode = p.orderCode,
				orderItems = (List<OrderItemsVM>)GetOrderItemsIndex(p.Id)
			});
			return Ok(result);
		}
		private static IEnumerable<OrderItemsVM> GetOrderItemsIndex(int orderId)
		{
			var db = new AppDbContext();


			var orderItems = db.orderItems
				.AsNoTracking()
				.Where(o => o.order_Id == orderId)
				.ToList()
				.Select(o => new OrderItemsVM
				{
					Id = o.Id,
					order_Id = o.order_Id,
					product_name = o.product_name,
					per_price = o.per_price,
					quantity = o.quantity,
					discount_name = o.discount_name,
					subtotal = o.subtotal,
					discount_subtotal = o.discount_subtotal,
					Items_description = o.Items_description,
					productcommit = o.productcommit,
					comment=o.comment,
				})
				.ToList();

			return orderItems;
		}
		[HttpPut("cancel")]
		public async Task<string> CancelOrders(int id)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}
			order emp = await _context.orders.FindAsync(id);


			if (emp.order_status_Id == 1 || emp.order_status_Id == 2)
			{
				DateTime currentTime = DateTime.Now;
				TimeSpan timeDifference = (TimeSpan)(emp.close_time - currentTime);

				if (timeDifference.TotalDays <= 7)
				{
					return "已過退費時間，無法取消";
				}
				emp.order_status_Id = 7;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				emp.close = true;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return "已取消訂單";
			}
			else
			{
				return "訂單已寄出，無法取消";

			}
		}
		[HttpPut("cancelcourse")]
		public async Task<string> Cancelcourse(int id)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}
			order emp = await _context.orders.FindAsync(id);


			if (emp.order_status_Id == 1 || emp.order_status_Id == 2)
			{
				emp.order_status_Id = 7;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				emp.close = true;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return "已取消預約";
			}
			else
			{
				return "無法取消";

			}
		}
		[HttpPut("cancelProduct")]
		public async Task<string> CancellProductOrders(int id)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}
			order emp = await _context.orders.FindAsync(id);


			if (emp.order_status_Id == 1 || emp.order_status_Id == 2)
			{
				emp.order_status_Id = 7;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				emp.close = true;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return "已取消訂單";
			}
			else
			{
				return "訂單已寄出，無法取消";

			}
		}

		[HttpPut("return")]
		public async Task<string> ReturnOrders(int orderid)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}
			order emp = await _context.orders.FindAsync(orderid);

			if (emp.order_status_Id == 6)
			{
				emp.order_status_Id = 9;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return "已申請退貨";
			}
			else
			{
				return "訂單尚未領取";
			}
		}
		[HttpPut("Change")]
		public async Task<string> ChangeOrders(int orderid)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}
			order emp = await _context.orders.FindAsync(orderid);

			if (emp.order_status_Id == 6)
			{
				emp.order_status_Id = 10;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return "已申請換貨";
			}
			else
			{
				return "訂單尚未領取";
			}
		}
		[HttpPut("cancelreturn")]
		public async Task<string> CancelReturn(int orderid)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}
			order emp = await _context.orders.FindAsync(orderid);

			if (emp.order_status_Id == 9)
			{
				emp.order_status_Id = 6;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return "已取消退貨";
			}
			else
			{
				return "訂單尚未領取";
			}

		}
		[HttpPut("cancelChange")]
		public async Task<string> CancelChange(int orderid)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}
			order emp = await _context.orders.FindAsync(orderid);

			if (emp.order_status_Id == 10)
			{
				emp.order_status_Id = 6;
				_context.Entry(emp).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return "已取消換貨";
			}
			else
			{
				return "訂單尚未領取";
			}

		}
		[HttpPut("setclose")]
		public async Task<string> Setclose(int orderid)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}
			//order emp = await _context.orders.FindAsync(orderid);

			await Task.Delay(TimeSpan.FromSeconds(10));
			order emp = await _context.orders.FindAsync(orderid);
			emp.close = true;
			_context.Entry(emp).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return "已過鑑賞期";

		}
		[HttpPost("NewReturn")]
		public async Task<string> Return(ReturnVM reDTO, int orderid)
		{
			var re = new Return
			{
				退貨日期 = DateTime.Now,
				fk訂單 = orderid,
				退貨轉帳帳號 = reDTO.退貨轉帳帳號,
				退款狀態 = false,
				退貨理由 = reDTO.退貨理由,
			};
			_context.Returns.Add(re);
			await _context.SaveChangesAsync();

			return "輸入成功";
		}
		[HttpGet("ReturnReason")]
		public async Task<IEnumerable<ReturnReaasonVM>> ReturnReason(int id)
		{
			var db = _context;
			var Reason = db.ReturnResons
				.AsNoTracking()
				.Where(x => x.ID == id)
				.Select(o => new ReturnReaasonVM
				{
					ID = o.ID,
					退貨理由 = o.退貨理由,
				});
			return Reason;
		}
		[HttpGet("ReturnReasons")]
		public async Task<IEnumerable<ReturnReaasonVM>> GetReturnReasons()
		{
			var db = _context;
			var Reasons = db.ReturnResons
				.AsNoTracking()
				.Select(o => new ReturnReaasonVM
				{
					ID = o.ID,
					退貨理由 = o.退貨理由,
				});
			return Reasons;
		}
		[HttpPut("activitycolse")]
		public async Task<string> Activitycolse()
		{
			var allOrders = await _context.orders.ToListAsync();

			// 獲取目前時間
			DateTime currentTime = DateTime.Now;

			foreach (var order in allOrders)
			{
				// 判斷是否有 close_time 值且該值小於目前時間
				if (order.close_time.HasValue && order.close_time.Value < currentTime)
				{
					// 將 order_status_Id 改成 6
					order.order_status_Id = 6;
				}
			}
			// 儲存變更回資料庫
			await _context.SaveChangesAsync();
			return "活動已結束";
		}

		[HttpPost("Newcommit")]
		public async Task<string> Newcommit(commitVM reDTO)
		{
			var re = new ProductComment
			{
				CreateTime = DateTime.Now,
				fk_MemberId = reDTO.MemberID,
				fk_ProductGroupId = reDTO.ProductId,
				Description = reDTO.Description,
				Status = false,
				Score = reDTO.Score,
			};
			_context.ProductComments.Add(re);
			await _context.SaveChangesAsync();

			return "評論成功";
		}
		[HttpPut("fincomment")]
		public async Task<string> fincomment(int orderid)
		{
			var db = _context;
			if (_context.orders == null)
			{
				return null;
			}

			orderItem emp = await _context.orderItems.FindAsync(orderid);
			emp.comment = true; 
			_context.Entry(emp).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return "返回訂單";

		}


		//public async Task Connect()
		//{
		//	if (HttpContext.WebSockets.IsWebSocketRequest)
		//	{
		//		using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
		//		await ProcessWebSocket(webSocket);
		//	}
		//	else
		//	{
		//		HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
		//	}
		//}

		//public async Task ProcessWebSocket(WebSocket webSocket)
		//{
		//	WebSockets.TryAdd(webSocket.GetHashCode(), webSocket);
		//	var buffer = new byte[1024 * 4];
		//	var res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
		//	string? UserName = null;
		//	while (!res.CloseStatus.HasValue)
		//	{
		//		UserName = "匿名";
		//		var cmd = Encoding.UTF8.GetString(buffer, 0, res.Count);
		//		JObject data = JObject.Parse(cmd);
		//		string? Name = Convert.ToString(data["userName"]);
		//		string? Message = $"{Convert.ToString(data["message"])} at {DateTime.Now}";
		//		if (!string.IsNullOrEmpty(Name))
		//		{
		//			UserName = Name;
		//		}
		//		Broadcast(JsonConvert.SerializeObject(new
		//		{
		//			userName = UserName,
		//			message = Message
		//		}));
		//		res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
		//	}
		//	await webSocket.CloseAsync(res.CloseStatus.Value, res.CloseStatusDescription, CancellationToken.None);
		//	WebSockets.TryRemove(webSocket.GetHashCode(), out var removed); // removed允許未宣告
		//	Broadcast(JsonConvert.SerializeObject(new
		//	{
		//		userName = UserName,
		//		message = "離開聊天室"
		//	}));
		//}

		//public void Broadcast(string message)
		//{
		//	var buff = Encoding.UTF8.GetBytes(message);
		//	var data = new ArraySegment<byte>(buff, 0, buff.Length);
		//	Parallel.ForEach(WebSockets.Values, async (webSocket) =>
		//	{
		//		if (webSocket.State == WebSocketState.Open)
		//			await webSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
		//	});
		//}
	}

}
