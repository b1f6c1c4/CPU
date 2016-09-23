`default_nettype none
module Input(
   input Clock,
   input Reset,
   input [3:0] H,
   output [3:0] V,
   input ack,
   inout [7:0] io
   );

   wire [15:0] key;
   wire [7:0] cmd, cmd_t;

   assign io = ack ? 8'bz : cmd;

   Input_keyboard keybd(.Clock(Clock), .Reset(Reset),
                        .H(H), .V(V), .res(key));

   Input_encoder enc(.Clock(Clock), .Reset(Reset),
                     .key(key), .cmd(cmd_t));

   Input_buffer buff(.Clock(Clock), .Reset(Reset),
                     .in_cmd(cmd_t),
                     .ack(ack), .out_cmd(cmd));

endmodule
