`default_nettype none
module Input_classifier(
   input Clock,
   input Reset,
   input btn,
   output short,
   output long
   );
   parameter crit = 12500000;

   localparam S_NP = 2'd0; // not pressed
   localparam S_PD = 2'd1; // pressed
   localparam S_TD = 2'd2; // long press trigged

   reg [1:0] state;
   reg [31:0] count;

   assign long = state == S_PD && ~|count;
   assign short = state == S_PD && ~btn;

   always @(posedge Clock, negedge Reset)
      if (~Reset)
         begin
            state <= S_NP;
            count <= crit;
         end
      else
         case (state)
            S_NP:
               if (btn)
                  begin
                     state <= S_PD;
                     count <= count - 1;
                  end
            S_PD:
               if (~btn)
                  state <= S_NP;
               else if (~|count)
                  state <= S_TD;
               else
                  count <= count - 1;
            S_TD:
               if (~btn)
                  begin
                     state <= S_NP;
                     count <= crit;
                  end
            default:
               begin
                  state <= S_NP;
                  count <= crit;
               end
         endcase

endmodule
