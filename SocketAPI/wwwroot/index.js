var vm = new Vue({
  el: "#app",
  data: {
    self: "",
    personnel: [],
    control: "",
    chess: [],
    downCount: 0
  },
  methods: {
    circleDown: function (col) {
      if (col.s == "b")
        return "circle circle-down";
      else if (col.s == "w")
        return "circle circle-down circle-white";
      else if (col.r ? col.r == 'b' : this.control == 'b')
        return "circle circle-hover";
      else if (col.r ? col.r == 'w' : this.control == 'w')
        return "circle circle-hover circle-white";
    },
    down: function (col) {
      if (!this.personnel || this.personnel.length < 2) {
        this.$message('参赛人员不足。');
        return;
      }
      if (this.control == "k") {
        console.log("观战模式不可落子");
        return;
      }
      if (this.downCount != 0) {
        this.$message('等待对方落子');
        return;
      }
      if (col.s) {
        this.$message('落在别的地方吧');
        return;
      }
      col.s = this.control;
      axios.post(`${baseUrl}/service/send?token=${UrlKey("room")}&control=${this.control}`, this.chess)
        .then(res => {
          if (res.data) {
            var m = this.control == "b" ? "黑方" : "白方";
            GameRestart(UrlKey("room"),`${m}胜，游戏结束。`);
          }
        });
      DownPiece(JSON.stringify(this.chess), UrlKey("room"));
    }
  },
  watch: {
    "personnel": function (val) {
      var i = val.indexOf(this.self);
      if (i == 0) {
        this.control = "b"; //执黑
        this.downCount = 0;
      } else if (i == 1) {
        this.control = "w"; // 执白
        this.downCount = 1;
      } else {
        this.control = "k"; // 观战
      }
    },
    downCount: function () {
      console.log(this.downCount);
    }
  },
  mounted: function () {
    var room = UrlKey("room");
    if (!room || room == "") {
      room = RandomToken();
      location.href += `?room=${room}`;

    } else {

    }
    init(room);
    this.chess = InitChess();
  }
});