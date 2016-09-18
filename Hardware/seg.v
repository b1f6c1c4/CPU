`default_nettype none
module seg(
   input CLK_seg,
   input [7:0] data_inH,
   input [7:0] data_inL,
   output [3:0] seg_sel,
   output [7:0] data_out
   );

   wire Clock = CLK_seg;
   wire Reset = 1'b1; // fake reset

   // internal nets
   wire [15:0] data = {data_inH,data_inL};
   wire [15:0] abs_data = data[15] ? ~data + 16'b1 : data;

   wire [3:0] bcd0, bcd1, bcd2, bcd3;
   wire [0:7] oct0t, oct1t, oct2t, oct3t;
   reg [0:7] oct0, oct1, oct2, oct3;

   // control
   always @(*)
      if (|bcd3)
         oct0 <= oct0t;
      else
         oct0 <= 8'b11111111;

   always @(*)
      if (|bcd3 || |bcd2)
         oct1 <= oct1t;
      else
         oct1 <= 8'b11111111;

   always @(*)
      if (|bcd3 || |bcd2 || |bcd1)
         oct2 <= oct2t;
      else
         oct2 <= 8'b11111111;

   always @(*)
      oct3 <= oct3t;

   // main modules
   Output_num_decoder dec0(.bcd(bcd3), .dot(1'b0), .oct(oct0t));
   Output_num_decoder dec1(.bcd(bcd2), .dot(1'b0), .oct(oct1t));
   Output_num_decoder dec2(.bcd(bcd1), .dot(1'b0), .oct(oct2t));
   Output_num_decoder dec3(.bcd(bcd0), .dot(1'b0), .oct(oct3t));

   Output_divider divi(.Clock(Clock), .Reset(Reset),
                       .data(abs_data),
                       .bcd0(bcd0), .bcd1(bcd1), .bcd2(bcd2), .bcd3(bcd3));

   Output_scanner scan(.Clock(Clock), .Reset(Reset),
                       .oct0(oct0), .oct1(oct1), .oct2(oct2), .oct3(oct3),
                       .SD(seg_sel), .SEG(data_out));

endmodule
