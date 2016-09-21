`default_nettype none
module Output_num_decoder(
   input [3:0] bcd,
   input dot,
   output reg [7:0] oct
   );

   always @(*)
      begin
         oct[7] <= ~dot;
         case (bcd)
            4'h0: oct[6:0] <= 7'b1000000;
            4'h1: oct[6:0] <= 7'b1111001;
            4'h2: oct[6:0] <= 7'b0100100;
            4'h3: oct[6:0] <= 7'b0110000;
            4'h4: oct[6:0] <= 7'b0011001;
            4'h5: oct[6:0] <= 7'b0010010;
            4'h6: oct[6:0] <= 7'b0000010;
            4'h7: oct[6:0] <= 7'b1111000;
            4'h8: oct[6:0] <= 7'b0000000;
            4'h9: oct[6:0] <= 7'b0010000;
            4'ha: oct[6:0] <= 7'b0001000;
            4'hb: oct[6:0] <= 7'b0000011;
            4'hc: oct[6:0] <= 7'b1000110;
            4'hd: oct[6:0] <= 7'b0100001;
            4'he: oct[6:0] <= 7'b0000110;
            4'hf: oct[6:0] <= 7'b0001110;
         endcase
      end

endmodule
