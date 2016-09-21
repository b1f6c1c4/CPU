`default_nettype none
module Hardware(
   input CLK,
   input RST,
   input [3:0] H,
   output [3:0] V,
   output [3:0] SD,
   output [7:0] SEG,
   output [7:0] LD,
   input [7:0] SB,
   output Buzz
   );

   // fundamental modules
   wire Clock, Reset;
   assign Clock = CLK;
   rst_recover rst(Clock, RST, Reset);

   // links
   wire [7:0] io_ena;
   wire in_finish;
   wire [2:0] in_op;
   wire [7:0] in_SRCH, in_SRCL, in_DSTH, in_DSTL;
   wire [7:0] ou_H, ou_L;
   wire [7:0] io_0, io_1, io_2, io_3;
   wire [7:0] io_4, io_5, io_6, io_7;

   // auxillary
   latch_buffer lat0(
      .Clock(Clock), .Reset(Reset),
      .en(io_ena[0]), .in(io_0), .out(ou_L));
   latch_buffer lat1(
      .Clock(Clock), .Reset(Reset),
      .en(io_ena[1]), .in(io_1), .out(ou_H));
   buffer buf2(.in(in_SRCL), .out(io_2), .en(~io_ena[2]));
   buffer buf3(.in(in_SRCH), .out(io_3), .en(~io_ena[3]));
   buffer buf4(.in(in_DSTL), .out(io_4), .en(~io_ena[4]));
   buffer buf5(.in(in_DSTH), .out(io_5), .en(~io_ena[5]));
   buffer buf6(.in({in_op,in_finish}), .out(io_6), .en(~io_ena[6]));
   latch_buffer lat7(
      .Clock(Clock), .Reset(Reset),
      .en(io_ena[7]), .in(io_7), .out({SB[6:0],Buzz}));

   // main modules
   CPU u(
      .Clock(Clock), .Reset(Reset),
      .io_ena(io_ena),
      .io_0(io_0), .io_1(io_1),
      .io_2(io_2), .io_3(io_3),
      .io_4(io_4), .io_5(io_5),
      .io_6(io_6), .io_7(io_7));

   key_scan in(
      .CLK(Clock), .RESET(Reset),
      .V1(H[3]), .V2(H[2]), .V3(H[1]), .V4(H[0]),
      .H1(V[3]), .H2(V[2]), .H3(V[1]), .H4(V[0]),
      .SRCH(in_SRCH), .SRCL(in_SRCL), .DSTH(in_DSTH), .DSTL(in_DSTL),
      .ALU_OP(in_op), .finish(in_finish));

   seg out(
      .CLK_seg(Clock),
      .data_inH(ou_H), .data_inL(ou_L),
      .seg_sel(SD), .data_out(SEG));

endmodule
