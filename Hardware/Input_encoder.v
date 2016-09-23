`default_nettype none
module Input_encoder(
   input Clock,
   input Reset,
   input [15:0] key,
   output reg [8:0] cmd
   );

   wire t[31:0];

   genvar i;
   generate
      for (i = 0; i < 16; i = i + 1)
         begin : GEN
            Input_classifier c_CTOK_CLCL(
               .Clock(Clock), .Reset(Reset),
               .btn(key[i]), .short(t[i]), .long(t[i+16]));
         end
   endgenerate

   integer j;
   always @(*)
      begin
         cmd = {8{1'b1}};
         for (j = 0; j < 32; j = j + 1)
            if (t[j])
               cmd = j;
      end

endmodule
