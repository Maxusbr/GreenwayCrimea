<div>[%= TopHtml %]</div>
<div id="divRelatedProductsInSc" class="pv-tile carousel-default">
	<ul class="jcarousel">
		[% for(var i=0, arrLength = RelatedProducts.length; i<arrLength; i++) { %]
		<li style="width: [%= ImageMaxWidth %]px;">
            <table class="p-table">
				<tr>
					<td class="img-middle" style="height: [%= ImageMaxHeight%]px;">
						<div class="pv-photo" onclick="location.href='[%= RelatedProducts[i].Link%]'">
							[%= RelatedProducts[i].Photo%]
						</div>
					</td>
				</tr>
				<tr>
					<td>
						<div>
							<a href="[%= RelatedProducts[i].Link%]" class="link-pv-name">[%= RelatedProducts[i].Name%]</a>
						</div>
						<div class="sku">[%= RelatedProducts[i].Sku%]</div>
						<div class="price-container">
							<div class='price'>[%= RelatedProducts[i].Price%]</div>
						</div>
						[%= RelatedProducts[i].Buttons%]
					</td>
				</tr>
			</table>
		</li>
		[% } %]
	</ul>
</div>
<div>[%= BottomHtml %]</div>