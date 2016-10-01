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
   output TX,
   input RX,
   output Buzz
`ifdef SIMULATION
   ,
   output [7:0] io_ena,
   inout [7:0] io_0,
   inout [7:0] io_1,
   inout [7:0] io_2,
   inout [7:0] io_3,
   inout [7:0] io_4,
   inout [7:0] io_5,
   inout [7:0] io_6,
   inout [7:0] io_7
`endif
   );

   // fundamental modules
   wire Clock, Reset;
   assign Clock = CLK;
   rst_recover rst(Clock, RST, Reset);

   // links
   wire [7:0] ou_L, ou_H;

   wire uw_ready, uw_send, uw_finish;
   wire [7:0] uw_data;

   wire ur_arrived;
   wire [7:0] ur_data;

   wire [2:0] baud;

`ifndef SIMULATION
   wire [7:0] io_ena;
   wire [7:0] io_0, io_1, io_2, io_3;
   wire [7:0] io_4, io_5, io_6, io_7;
`endif

   // auxillary
   latch_buffer lat0(
      .Clock(Clock), .Reset(Reset),
      .en(io_ena[0]), .in(io_0), .out(ou_L));
   latch_buffer lat1(
      .Clock(Clock), .Reset(Reset),
      .en(io_ena[1]), .in(io_1), .out(ou_H));
   latch_buffer lat7(
      .Clock(Clock), .Reset(Reset),
      .en(io_ena[7]), .in(io_7), .out(LD));
   opaque_read_buffer orb(
      .Clock(Clock), .Reset(Reset),
      .Din(ur_data), .Din_arrived(ur_arrived),
      .io(io_3), .ena(io_ena[3]));
   opaque_write_buffer owb(
      .Clock(Clock), .Reset(Reset),
      .Dout(uw_data), .Dout_send(uw_send),
      .Dout_ready(uw_ready), .Dout_finish(uw_finish),
      .io(io_4), .ena(io_ena[4]));

   assign Buzz = ~LD[0];

   assign baud = SB == 8'h0 ? 3'h4 : SB == 8'h1 ? 3'h2 : SB == 8'h2 ? 3'h1 : 3'h4;
   assign io_6 = io_ena[6] ? 8'hz : SB;

   // main modules
   CPU u(
      .Clock(Clock), .Reset(Reset),
      .io_ena(io_ena),
      .io_0(io_0), .io_1(io_1),
      .io_2(io_2), .io_3(io_3),
      .io_4(io_4), .io_5(io_5),
      .io_6(io_6), .io_7(io_7));

   Input in(
      .Clock(Clock), .Reset(Reset),
      .H(H), .V(V), .ack(io_ena[2]),
      .io(io_2));

   seg out(
      .CLK_seg(Clock),
      .data_inH(ou_H), .data_inL(ou_L),
      .seg_sel(SD), .data_out(SEG));

   UART_WriteD uw(
      .Clock(Clock), .Reset(Reset),
      .Baud(baud),
      .ready(uw_ready), .send(uw_send),
      .finish(uw_finish), .data(uw_data),
      .TX(TX));

   UART_ReadD ur(
      .Clock(Clock), .Reset(Reset),
      .Baud(baud),
      .arrived(ur_arrived), .data(ur_data),
      .RX(RX));

endmodule
