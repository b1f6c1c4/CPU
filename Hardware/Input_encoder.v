`default_nettype none
module Input_encoder(
   input Clock,
   input Reset,
   input [15:0] key,
   output reg [IC_N-1:0] cmd
   );
`include "INPUT_INTERNAL.v"
`include "INPUT_INTERFACE.v"

   wire NUM1, NUM2, NUM3, NUM4, NUM5, NUM6, NUM7, NUM8, NUM9, NUM0;
   wire OPAD, OPSB, OPAN, OPOR, OPLS, CTOK;

   Input_diff d_NUM1(.Clock(Clock), .Reset(Reset),
                     .btn(key[15]), .pressed(NUM1));
   Input_diff d_NUM2(.Clock(Clock), .Reset(Reset),
                     .btn(key[11]), .pressed(NUM2));
   Input_diff d_NUM3(.Clock(Clock), .Reset(Reset),
                     .btn(key[7]), .pressed(NUM3));
   Input_diff d_NUM4(.Clock(Clock), .Reset(Reset),
                     .btn(key[3]), .pressed(NUM4));
   Input_diff d_NUM5(.Clock(Clock), .Reset(Reset),
                     .btn(key[14]), .pressed(NUM5));
   Input_diff d_NUM6(.Clock(Clock), .Reset(Reset),
                     .btn(key[10]), .pressed(NUM6));
   Input_diff d_NUM7(.Clock(Clock), .Reset(Reset),
                     .btn(key[6]), .pressed(NUM7));
   Input_diff d_NUM8(.Clock(Clock), .Reset(Reset),
                     .btn(key[2]), .pressed(NUM8));
   Input_diff d_NUM9(.Clock(Clock), .Reset(Reset),
                     .btn(key[13]), .pressed(NUM9));
   Input_diff d_NUM0(.Clock(Clock), .Reset(Reset),
                     .btn(key[9]), .pressed(NUM0));
   Input_diff d_OPAD(.Clock(Clock), .Reset(Reset),
                     .btn(key[5]), .pressed(OPAD));
   Input_diff d_OPSB(.Clock(Clock), .Reset(Reset),
                     .btn(key[1]), .pressed(OPSB));
   Input_diff d_OPAN(.Clock(Clock), .Reset(Reset),
                     .btn(key[12]), .pressed(OPAN));
   Input_diff d_OPOR(.Clock(Clock), .Reset(Reset),
                     .btn(key[8]), .pressed(OPOR));
   Input_diff d_OPLS(.Clock(Clock), .Reset(Reset),
                     .btn(key[4]), .pressed(OPLS));
   Input_diff d_CTOK(.Clock(Clock), .Reset(Reset),
                     .btn(key[0]), .pressed(CTOK));

   always @(*)
      case (1'b1)
         CTOK: cmd <= IC_CTOK;
         OPAN: cmd <= IC_OPAN;
         OPOR: cmd <= IC_OPOR;
         OPAD: cmd <= IC_OPAD;
         OPSB: cmd <= IC_OPSB;
         OPLS: cmd <= IC_OPLS;
         NUM9: cmd <= IC_NUM9;
         NUM8: cmd <= IC_NUM8;
         NUM7: cmd <= IC_NUM7;
         NUM6: cmd <= IC_NUM6;
         NUM5: cmd <= IC_NUM5;
         NUM4: cmd <= IC_NUM4;
         NUM3: cmd <= IC_NUM3;
         NUM2: cmd <= IC_NUM2;
         NUM1: cmd <= IC_NUM1;
         NUM0: cmd <= IC_NUM0;
         default: cmd <= IC_NONE;
      endcase

endmodule
