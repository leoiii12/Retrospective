﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using PusherServer;
using Retrospective.Boards.Interfaces;
using Retrospective.Common;
using Retrospective.Events;
using Retrospective.Functions.Dtos;

namespace Retrospective.Functions
{
    public class CreateBoardItemFunction : IFunction<CreateBoardItemInput>
    {
        private readonly IBoardManager _boardManager;
        private readonly IPusher _pusher;

        public CreateBoardItemFunction(
            IPusher pusher,
            IBoardManager boardManager)
        {
            _pusher = pusher;
            _boardManager = boardManager;
        }

        public async Task InvokeAsync(CreateBoardItemInput input, TraceWriter log)
        {
            var board = await _boardManager.GetAsync(input.BoardId, input.Password) ?? throw new UserFriendlyException("The board does not exist.");

            var triggerResult = await _pusher.TriggerAsync(board.ToString(), BoardItem_Create.EventName, new BoardItem_Create {Type = input.Type});
            if (triggerResult.StatusCode != HttpStatusCode.OK) throw new ApplicationException($"Cannot publish {BoardItem_Create.EventName}. {JsonConvert.SerializeObject(triggerResult)}");
        }
    }
}