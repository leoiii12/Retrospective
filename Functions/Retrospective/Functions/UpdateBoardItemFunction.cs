﻿using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PusherServer;
using Retrospective.Boards.Interfaces;
using Retrospective.Common;
using Retrospective.Events;
using Retrospective.Functions.Dtos;

namespace Retrospective.Functions
{
    public class UpdateBoardItemFunction : IFunction<UpdateBoardItemInput>
    {
        private readonly IBoardManager _boardManager;
        private readonly IPusher _pusher;

        public UpdateBoardItemFunction(
            IPusher pusher,
            IBoardManager boardManager)
        {
            _pusher = pusher;
            _boardManager = boardManager;
        }

        public async Task InvokeAsync(UpdateBoardItemInput input)
        {
            var board = await _boardManager.GetAsync(input.BoardId, input.Password);
            if (board == null) throw new Exception("The board does not exist.");

            var triggerResult = await _pusher.TriggerAsync(board.ToString(), "BoardItem-Update", new BoardItem_Update {Id = input.Id, Title = input.Title, Content = input.Content, Type = input.Type});
            if (triggerResult.StatusCode != HttpStatusCode.OK) throw new Exception("Cannot publish \"BoardItem-Create\". " + JsonConvert.SerializeObject(triggerResult));
        }
    }
}