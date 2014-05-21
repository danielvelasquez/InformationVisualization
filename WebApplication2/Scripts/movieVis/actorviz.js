/*var custom_bubble_chart = (function(d3, CustomTooltip) {
  "use strict";

  var width = 940,
      height = 600,
      panwidth = width + 40,
      panheight = height + 40,
      tooltip = CustomTooltip("gates_tooltip", 240),
      layout_gravity = -0.01,
      damper = 0.1,
      nodes = [],
      vis, force, textcloud, size_scale, max_score;
	  
	  
var w = 940,
	h = 600;

var maxDataPointsForDots = 50,
	transitionDuration = 1000;

var svg = null,
	yAxisGroup = null,
	xAxisGroup = null,
	dataCirclesGroup = null,
	dataLinesGroup = null;

  var center = {x: width / 2, y: (height / 2)-50};

  var find_trending_actors = function() {
	
  }
  
  var draw_trendChart = function() {
  }

  var fill_color = d3.scale.ordinal()
                  .domain([1, 2, 3, 4, 5])
                  .range(["#ec008c", "#00aeef", "#7ac143", "#f47b20", "#2e3192"]);

  function custom_chart(data) {
    
	
	max_score = d3.max(data, function(d) { return parseInt(d.Score, 10); } );
    size_scale = d3.scale.pow().exponent(0.5).domain([0, max_score]).range([2, 60]);
	//radius_scale = d3.scale.linear().domain([0, max_amount]).range([2, 60]);
	var getColor = function(e) {
	var tmp = Math.floor(e/12);
	return fill_color(tmp);
    //return "hsl(" + Math.random() * 360 + ",100%,50%)";
    }
    //create node objects from original data
    //that will serve as the data behind each
    //bubble in the vis, then add each node
    //to nodes to be used later
    data.forEach(function(d){
     
      var node = {
        id: d.id,
        name: d.ActName ,
        interval: d.Interval,
        img: d.Profile_Img_Path,
        score: parseInt(d.Score,10),
        size: size_scale(parseInt(d.Score,10)),
        color: getColor(parseInt(d.Score,10)),
        totalnumfilms:parseInt(d.I,10),
        x: Math.random() * 900,
        y: Math.random() * 800
      };
      
      nodes.push(node);
    });

    nodes.sort(function(a, b) {return b.value- a.value; });

    vis = d3.select("#vis").append("svg")
                .attr("width", panwidth)
                .attr("height", panheight)
                .attr("id", "svg_vis")
				.attr("class", "svg_vis")
                .attr("style", "margin-left:10px");
    
    
    textcloud = vis.selectAll("text")
                 .data(nodes, function(d) { return d.id ;})
                 .enter()
                 .append("text");
	
	
	var textLabels = textcloud
                 .attr("x", function(d) { return d.x; })
                 .attr("y", function(d) { return d.y; })
                 .text( function (d) { return d.name; })
                 .attr("font-family", "sans-serif")
				 .attr("class","textcloud")
                 .attr("font-size", function(d) { return "1px"; })
                 .attr("fill", function (d) { return d.color; })
                 .on("mouseover", function(d, i) {show_details(d, i, this);} )
      			 .on("mouseout", function(d, i) {hide_details(d, i, this);} );
	
	
    textcloud.transition().duration(2000).attr("font-size", function(d) { return d.size+"px"; });
	

  }

  function charge(d) {
     
    return -Math.pow(d.score, 2.5) / 8;
  }

  function start() {
    force = d3.layout.force()
            .nodes(nodes)
            .size([width, height]);
  }

  function display_group_all() {
    force.gravity(layout_gravity)
         .charge(charge)
         .friction(0.8)
         .on("tick", function(e) {
            textcloud.each(move_towards_center(e.alpha))
                   .attr("x", function(d) {return d.x;})
                   .attr("y", function(d) {return d.y;});
         });
    force.start();
    //hide_years();
	//hide_grid();
  }

  function move_towards_center(alpha) {
    return function(d) {
      d.x = d.x + (center.x - d.x) * (damper + 0.02) * alpha;
      d.y = d.y + (center.y - d.y) * (damper + 0.02) * alpha;
    };
  }

  
  

 


function draw() {
	var mdata = generateData();
	var m = 40;
	var max = d3.max(mdata, function(d) { return d.value });
	var min = 0;
	var pointRadius = 4;
	var xScale = d3.time.scale().range([0, w - m * 2]).domain([mdata[0].date, mdata[mdata.length - 1].date]);
	var yScale = d3.scale.linear().range([h - m * 2, 0]).domain([min, max]);

	var xAxis = d3.svg.axis().scale(xScale).tickSize(h - m * 2).tickPadding(10).ticks(7);
	var yAxis = d3.svg.axis().scale(yScale).orient('left').tickSize(-w + m * 2).tickPadding(10);
	var t = null;

	svg = d3.select('#chart').select('svg').select('g');
	if (svg.empty()) {
		svg = d3.select('#chart')
			.append('svg:svg')
				.attr('width', w)
				.attr('height', h)
				.attr('class', 'viz')
			.append('svg:g')
				.attr('transform', 'translate(' + m + ',' + m + ')');
	}

	t = svg.transition().duration(transitionDuration);

	// y ticks and labels
	if (!yAxisGroup) {
		yAxisGroup = svg.append('svg:g')
			.attr('class', 'yTick')
			.call(yAxis);
	}
	else {
		t.select('.yTick').call(yAxis);
	}

	// x ticks and labels
	if (!xAxisGroup) {
		xAxisGroup = svg.append('svg:g')
			.attr('class', 'xTick')
			.call(xAxis);
	}
	else {
		t.select('.xTick').call(xAxis);
	}

	// Draw the lines
	if (!dataLinesGroup) {
		dataLinesGroup = svg.append('svg:g');
	}

	var dataLines = dataLinesGroup.selectAll('.data-line')
		.data(mdata);

	dataLines
		.enter()
		.append('svg:line')
			.attr('class', 'data-line')
			.style('opacity', 1e-6)
			.attr('x1', function(d, i) { return (i > 0) ? xScale(mdata[i - 1].date) : xScale(d.date); })
			.attr('y1', function() { return yScale(0); })
			.attr('x2', function(d) { return xScale(d.date); })
			.attr('y2', function() { return yScale(0); })
		.transition()
		.delay(transitionDuration / 2)
		.duration(transitionDuration)
			.style('opacity', 1)
			.attr('x1', function(d, i) { return (i > 0) ? xScale(mdata[i - 1].date) : xScale(d.date); })
			.attr('y1', function(d, i) { return (i > 0) ? yScale(mdata[i - 1].value) : yScale(d.value); })
			.attr('x2', function(d) { return xScale(d.date); })
			.attr('y2', function(d) { return yScale(d.value); });

	dataLines
		.transition()
		.duration(transitionDuration)
			.style('opacity', 1)
			.attr('x1', function(d, i) { return (i > 0) ? xScale(mdata[i - 1].date) : xScale(d.date); })
			.attr('y1', function(d, i) { return (i > 0) ? yScale(mdata[i - 1].value) : yScale(d.value); })
			.attr('x2', function(d) { return xScale(d.date); })
			.attr('y2', function(d) { return yScale(d.value); });

	dataLines
		.exit()
		.transition()
		.duration(transitionDuration)
			.style('opacity', 1e-6)
			.attr('y1', function() { return yScale(0); })
			.attr('y2', function() { return yScale(0); })
			.remove();

	// Draw the points
	if (!dataCirclesGroup) {
		dataCirclesGroup = svg.append('svg:g');
	}

	var circles = dataCirclesGroup.selectAll('.data-point')
		.data(mdata);

	circles
		.enter()
			.append('svg:circle')
				.attr('class', 'data-point')
				.style('opacity', 1e-6)
				.attr('cx', function(d) { return xScale(d.date) })
				.attr('cy', function() { return yScale(0) })
				.attr('r', function() { return (mdata.length <= maxDataPointsForDots) ? pointRadius : 0 })
			.transition()
			.duration(transitionDuration)
				.style('opacity', 1)
				.attr('cx', function(d) { return xScale(d.date) })
				.attr('cy', function(d) { return yScale(d.value) });

	circles
		.transition()
		.duration(transitionDuration)
			.attr('cx', function(d) { return xScale(d.date) })
			.attr('cy', function(d) { return yScale(d.value) })
			.attr('r', function() { return (mdata.length <= maxDataPointsForDots) ? pointRadius : 0 })
			.style('opacity', 1);

	circles
		.exit()
			.transition()
			.duration(transitionDuration)
				// Leave the cx transition off. Allowing the points to fall where they lie is best.
				//.attr('cx', function(d, i) { return xScale(i) })
				.attr('cy', function() { return yScale(0) })
				.style("opacity", 1e-6)
				.remove();
}

function generateData() {
	var dataset = [];
	var data = [];
	
	var i = 0;
for(var j=0;j<5;j++){
	i = Math.max(Math.round(Math.random()*100), 3);
	while (i--) {
		var date = new Date();
		date.setDate(date.getDate() - i);
		date.setHours(0, 0, 0, 0);
		data.push({'value' : Math.round(Math.random()*1200), 'date' : date});
	}
	if(j<4)
	data = [];
	dataset.push(data);
}
	
	return data;
}

  
  
var clear_screan = function() {
	 var test = vis.selectAll(".svg_vis").remove();
}
  
  


// the tool tip ****************************************************************************************
  function show_details(data, i, element) {
    d3.select(element).attr("stroke", "black");
    var tmpscore,n,f;
    var star_scaler = d3.scale.pow().exponent(0.5).domain([0, max_score]).range([0, 5]);
    tmpscore = star_scaler(data.score);
	n = Math.floor(tmpscore);
	f = tmpscore - n;
    var content = "<div class='tooltipimg'><img src=" + data.img + "alt='Cover Image' height='42' width='42'></div>";
    content += "<div class='tooltipcontent'><span class=\"value\"> " + data.name + "</span><br/>";
    content +="<span class=\"value\"> " + data.interval + "</span><br/>";
    content +="<span class=\"value\"> " + data.score + "/" + max_score + "</span><br/>";
    for(var i=0;i<n;i++)
    	content +="<img src='img/star-on.png' alt='star' height='10' width='10'>";
    if(f > 0)
    	{
    		content +="<img src='img/star-half.png' alt='star' height='10' width='10'>";
    		f=1;
    	}
	for(var i=0;i <(5-n-f);i++)
    	content +="<img src='img/star-off.png' alt='star' height='10' width='10'>";
    content +="<br/><span style='color:red' class=\"value\"> " + data.value + " votes</span></div>";
    tooltip.showTooltip(content, d3.event);
  }

  function hide_details(data, i, element) {
    d3.select(element).attr("stroke", function(d) { return d3.rgb(fill_color(d.group)).darker();} );
    tooltip.hideTooltip();
  }

  var my_mod = {};
  my_mod.init = function (_data) {
  //debugger
    custom_chart(_data);
    start();
  };

  my_mod.display_all = display_group_all;
  //my_mod.display_year = display_by_year;
  my_mod.toggle_view = function(view_type) {
    if (view_type == 'topactor') {
      //clear_screen();
      display_group_all();
    } else if(view_type == 'actorProgress'){
      debugger
	  clear_screan();
      }else {
      display_group_all();
      }
    };

  return my_mod;
})(d3, CustomTooltip);*/