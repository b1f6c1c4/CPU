`default_nettype none
module Output_num_decoder(
   input [3:0] bcd,
   input dot,
   output reg [0:7] oct
   );

   always @(*)
      begin
         oct[7] <= ~dot;
         case (bcd)
            4'h0: oct[0:6] <= 7'b0000001;
            4'h1: oct[0:6] <= 7'b1001111;
            4'h2: oct[0:6] <= 7'b0010010;
            4'h3: oct[0:6] <= 7'b0000110;
            4'h4: oct[0:6] <= 7'b1001100;
            4'h5: oct[0:6] <= 7'b0100100;
            4'h6: oct[0:6] <= 7'b0100000;
            4'h7: oct[0:6] <= 7'b0001111;
            4'h8: oct[0:6] <= 7'b0000000;
            4'h9: oct[0:6] <= 7'b0000100;
            4'ha: oct[0:6] <= 7'b0001000;
            4'hb: oct[0:6] <= 7'b1100000;
            4'hc: oct[0:6] <= 7'b0110001;
            4'hd: oct[0:6] <= 7'b1000010;
            4'he: oct[0:6] <= 7'b0110000;
            4'hf: oct[0:6] <= 7'b0111000;
         endcase
      end

endmodule
