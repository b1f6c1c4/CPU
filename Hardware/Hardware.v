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
   wire in_finish;
   wire [2:0] in_op;
   wire [7:0] in_SRCH, in_SRCL, in_DSTH, in_DSTL;
   wire [7:0] ou_H, ou_L;

   // auxillary

   // main modules
   CPU u(
      .Clock(Clock), .Reset(Reset),
      .io_0(ou_H), .io_1(ou_L),
      .io_2(in_SRCH), .io_3(in_SRCL),
      .io_4(in_DSTH), .io_5(in_DSTL),
      .io_6(in_op));

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
