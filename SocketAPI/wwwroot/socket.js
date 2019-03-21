var connection;
function init(room) {
  connection = new signalR.HubConnectionBuilder()
    .withUrl(`http://118.24.27.231:5001/ws?token=${room}`)
    .configureLogging(signalR.LogLevel.Warning)
    .build();

  connection.start().then(function () {
    console.log("connected");
    window.setInterval(_ => {
      connection.invoke("ping");
    }, 7000);
  });

  connection.on("Send", msg => {
    console.log(msg);
  });

  connection.on("AllOnLine", msg => {
    vm.personnel = msg;
  });

  connection.on("ListenSelf", msg => {
    vm.self = msg;
  });

  connection.on("GameRestart", _ => {
    alert("参战人员离开，游戏结束");
    vm.chess = InitChess();
  });

  connection.on("Pong", _ => {
    console.log("pong");
  });

  connection.on("DownPieceMsg", msg => {
    vm.chess = JSON.parse(msg);
    console.log(vm.chess);
    vm.downCount++;
    if (vm.downCount >= 2) {
      vm.downCount = 0;
    }
  });
}

function InitChess() {
  var chess = [];
  for (var i = 0; i < 13; i++) {
    var temp = [];
    for (var j = 0; j < 13; j++) {
      temp.push({
        c: [i, j].toString(), // 位置
        s: "",                // 是否有子
        r: ""                 // 属于谁
      });
    }
    chess.push(temp);
  }
  return chess;
}

function DownPiece(token, msg) {
  // axios.get(`http://localhost:5001/service/send?token=${token}&msg=${msg}`)
  //   .then(x => {
  //     console.log(x);
  //   })
  //   .catch(e => {
  //     console.log(e);
  //   });
  connection.invoke("DownPiece", msg, token)
    .catch(err => console.error(err.toString()));
}

function RandomToken() {
  var seed = [1, 2, 3, 4, 5, 6, 7, "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "l", "q", "w", "e", "r", "t", "y", "u", "i", "x", "b", "c", "b", "n", "o", "m", 0];
  var r = "";
  for (var i = 0; i < 11; i++) {
    r += seed[Math.round(Math.random() * 100 / 3)];
  }
  return r;
}

function UrlKey(name) {
  return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.href) || [, ""])[1].replace(/\+/g, '%20')) || null;
}

Array.prototype.indexOf = function (val) {
  for (var i = 0; i < this.length; i++) {
    if (this[i] == val) return i;
  }
  return -1;
};

Array.prototype.remove = function (val) {
  var index = this.indexOf(val);
  if (index > -1) {
    this.splice(index, 1);
  }
};