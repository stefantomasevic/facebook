using AutoMapper;
using Facebook.Data;
using Facebook.DTO;
using Facebook.Hubs;
using Facebook.Models;
using Facebook.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace Facebook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IMessageRepository _repository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserRepository _userRepository;

        public MessageController(IMessageRepository messageRepository, IMapper mapper, ApplicationDbContext context, IHubContext<ChatHub> hubContext, IUserRepository userRepository)
        {
            _repository = messageRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            _context = context;
            _userRepository = userRepository;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] MessageDTO messageDTO)
        {
            try
            {
                var message = _mapper.Map<Message>(messageDTO);

                await _repository.AddMessage(message);
                if(message != null)
                {
                    //send message to user 
                   var sender= await _userRepository.GetByIdAsync(message.SenderID);
                    var recevier= await _userRepository.GetByIdAsync(message.ReceiverID);
                    if(sender != null && recevier != null)
                    {
                        //send message to user by username, everything you neeed on front is listening this hub SIGNALR library

                        await _hubContext.Clients.User(recevier.Username).SendAsync("ReceiveMessage", sender.Username, message);
                    }
                }

                return Ok("Poruka uspešno poslata.");
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("messagesBetweenUsers/{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetMessagesBetweenUsers(int user1Id, int user2Id)
        {
            try
            {
                var userMessages = await _repository.GetMessagesBetweenUsers(user1Id, user2Id);

                var sender = await _context.Users.FindAsync(user1Id);
                var receiver = await _context.Users.FindAsync(user2Id);

                var userMessageDTO = new UserMessageDTO
                {
                    SenderID = user1Id,
                    SenderUsername = sender.Username, //
                    ReceiverID = user2Id,
                    ReceiverUsername = receiver.Username, // 
                    Messages = new List<MessageDetailDTO>()
                };

                foreach (var message in userMessages)
                {
                    var messageDTO = new MessageDetailDTO
                    {
                       Text=message.MessageText,
                       Image=message.Image,
                       SenderID=message.SenderID,
                       ReceiverID=message.ReceiverID,
                       Time=message.SendingDate

                    };

                    userMessageDTO.Messages.Add(messageDTO);
                }


                return Ok(userMessageDTO);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("deleteMessage/{messageId}")]
        public async Task<IActionResult> DeleteMessage(int messageId)
        {
            try
            {
                var deleted = await _repository.DeleteMessageAsync(messageId);

                if (deleted)
                {
                    return Ok("Message deleted successfully.");
                }
                else
                {
                    return NotFound("Message not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
