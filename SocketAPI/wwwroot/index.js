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
    circleDown: function (s) {
      if (s) {
        return "circle circle-down";
      }
      else {
        return "circle circle-hover";
      }
    },
    down: function (col) {
      if (col.s) {
        console.warn("已有子位置，落子验证。");
      }
      col.s = true;
      DownPiece(JSON.stringify(this.chess), UrlKey("room"));
    }
  },
  watch: {
    "personnel": function (val) {
      var i = val.indexOf(this.self);
      if (i == 0) {
        this.control = "b"; //执黑
      } else if (i == 1) {
        this.control = "w"; // 执白
      } else {
        this.control = "k"; // 观战
      }
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
    for (var i = 0; i < 13; i++) {
      var temp = [];
      for (var j = 0; j < 13; j++) {
        temp.push({
          c: [i, j].toString(), // 位置
          s: false,             // 是否有子
          r: this.control       // 属于谁
        });
      }
      this.chess.push(temp);
    }
  }
});