   document.getElementsByClassName = function(cl) {
		var retnode = [];
		var myclass = new RegExp('\\b'+cl+'\\b');
		var elem = this.getElementsByTagName('*');
		for (var i = 0; i < elem.length; i++) {
		var classes = elem[i].className;
		if (myclass.test(classes)) retnode.push(elem[i]);
		}
		return retnode;
		};
   

var PanelDetails=function() {
    
    var slideTime=1;
    
	//initialize the values
    function slider(n) { 
		this.isAllOpen=true;
		this.accordionComponent=n; 
		this.listOfShow=[];
		this.listOfHide=[];
	}
    
	//create a accordion from the given component
    slider.prototype.init=function(componentObject,initialOpenIndex) {
        
        var i;
        
		var accordionObject=document.getElementById(componentObject); 
        
        var showbutton=accordionObject.getElementsByClassName('showaccord');
        
		var panelContentList=accordionObject.getElementsByClassName('panelcontent'); 
		
		var hidebutton=accordionObject.getElementsByClassName('hideaccord'); 
        
        for(i=0;i<showbutton.length;i++) {
            
			var show=showbutton[i];
			var hide=hidebutton[i];
            
			this.listOfShow[i]=show; 
			this.listOfHide[i]=hide; 
            
			show.onclick=new Function(this.accordionComponent+'.openOrClose(this)');
			hide.onclick=new Function(this.accordionComponent+'.ClosePanel(this)');
		}
        for(i=0;i<panelContentList.length;i++) {
            
			var panelContent=panelContentList[i]; 
            
			panelContent.maxheight=panelContent.offsetHeight; 
            
			if(initialOpenIndex!=i) { 
                
				panelContent.style.height=0;
				panelContent.style.display='none';
                
			}
		}
    }
    
	//slidding the accordion tab
	
	slider.prototype.ClosePanel=function(clickedPanel) {
			
		var j = 0;
		var k = 0;
		for(var i=0; i < this.listOfHide.length;i++) {
			
			var panel=this.listOfHide[i]; 
            
			panelContent=panel.parentNode; 
			           
			panelContent=panelContent.nodeType!=1?panelContent.nextSibling:panelContent;
            
			if(panelContent.style.display=='none'||panelContent.style.display=='block'){
				
				if((panel==clickedPanel && panelContent.style.display=='block') || panelContent.style.display=='none')
				{
					j++;
				}
				else if((panel==clickedPanel && panelContent.style.display=='none')|| panelContent.style.display=='block')
				{
					k++;
				}
			}
		}
		var total = this.listOfShow.length;
		if(j== this.listOfShow.length){
			this.closeAll();
			this.isAllOpen = false;
			
		}
		else if(k==this.listOfShow.length){
			this.openAll();
			this.isAllOpen = true;
		}
		
		
        for(var i=0;i<this.listOfShow.length;i++) {
            
            var panel=this.listOfShow[i]; 
            
			panelContent=panel.parentNode.nextSibling; 
            
			panelContent=panelContent.nodeType!=1?panelContent.nextSibling:panelContent; 
            
			//clearInterval(panelContent.slideTime);
            
            if(panel==clickedPanel&&panelContent.style.display=='none') { 
                
				slider.scrollTop=0;
                
				panelContent.style.display='block';
                
				slide(panelContent,1);
				panel.className+=" open";
                
			} 
            
            else if(this.isAllOpen&&panel==clickedPanel&&panelContent.style.display=='block') {
                
				slide(panelContent,1); 
                
				panelContent.style.display='block'; 
				panel.className+=" open";
			}
            
            else if(!this.isAllOpen&&panel!=clickedPanel){
                
                slide(panelContent,-1); 
                
                panelContent.style.display='none'; 
				panel.className=panel.className.replace( /(?:^|\s)open(?!\S)/g , '' );
				panel.parentNode.style.borderBottom="#5f879f solid 1px";
                
            }
			else if(panel==clickedPanel&&panelContent.style.display=='block'){
                
                slide(panelContent,-1); 
                
                panelContent.style.display='none'; 
				panel.className=panel.className.replace( /(?:^|\s)open(?!\S)/g , '' );
				panel.parentNode.style.borderBottom="#5f879f solid 1px";
                
            }
		}
		
	
	}
    
    slider.prototype.openOrClose=function(clickedPanel) {
        
		var j = 0;
		var k = 0;
		for(var i=0; i < this.listOfShow.length;i++) {
			
			var panel=this.listOfShow[i]; 
            
			panelContent=panel.parentNode.nextSibling; 
			           
			panelContent=panelContent.nodeType!=1?panelContent.nextSibling:panelContent;
            
			if(panelContent.style.display=='none'||panelContent.style.display=='block'){
				
				if((panel==clickedPanel && panelContent.style.display=='block') || panelContent.style.display=='none')
				{
					j++;
				}
				else if((panel==clickedPanel && panelContent.style.display=='none')|| panelContent.style.display=='block')
				{
					k++;
				}
			}
		}
		var total = this.listOfShow.length;
		if(j== this.listOfShow.length){
			this.closeAll();
			this.isAllOpen = false;
			
		}
		else if(k==this.listOfShow.length){
			this.openAll();
			this.isAllOpen = true;
		}
		
		
        for(var i=0;i<this.listOfShow.length;i++) {
            
            var panel=this.listOfShow[i]; 
            
			panelContent=panel.parentNode.nextSibling; 
            
			panelContent=panelContent.nodeType!=1?panelContent.nextSibling:panelContent; 
            
			//clearInterval(panelContent.slideTime);
            
            if(panel==clickedPanel&&panelContent.style.display=='none') { 
                
				slider.scrollTop=0;
                
				panelContent.style.display='block';
				panelContent.style.borderBottom='#5F879F solid 1px';
                
				slide(panelContent,1);
				panel.className+=" open";
				panel.parentNode.style.border="none";
                
			} 
            
            else if(this.isAllOpen&&panel==clickedPanel&&panelContent.style.display=='block') {
                
				slide(panelContent,1); 
                
				panelContent.style.display='block'; 
				panelContent.style.border="none";
				panel.className+=" open";
				panel.parentNode.style.borderBottom="#5f879f solid 1px";
			}
            
            else if(!this.isAllOpen&&panel!=clickedPanel){
                
                slide(panelContent,-1); 
                
                panelContent.style.display='none'; 
				panel.parentNode.style.borderBottom="#5f879f solid 1px";
				panel.className=panel.className.replace( /(?:^|\s)open(?!\S)/g , '' );
				
                
            }
			else if(panel==clickedPanel&&panelContent.style.display=='block'){
                
                slide(panelContent,-1); 
                
                panelContent.style.display='none'; 
				panel.className=panel.className.replace( /(?:^|\s)open(?!\S)/g , '' );
				panel.parentNode.style.borderBottom="#5f879f solid 1px";
                
            }
		}
    }
    
	//open all the tabs in accordion
	slider.prototype.openAll=function() {
        
        for(var i=0;i<this.listOfShow.length;i++) {
            
            var panel=this.listOfShow[i];
            
			panelContent=panel.nextSibling; 
            
			panelContent=panelContent.nodeType!=1?panelContent.nextSibling:panelContent; 
            
			//clearInterval(panelContent.slideTime);
            
            if(panelContent.style.display=='none') { 
                
				panelContent.style.display='block'; 
                
				slide(panelContent,1); 
                
				
			}
        }
    }
    
	//close all the tabs in accordion
    
	slider.prototype.closeAll=function() {
        
		for(var i=0;i<this.listOfShow.length;i++) {
            
            var panel=this.listOfShow[i];
            
			panelContent=panel.parentNode.nextSibling; 
            
			panelContent=panelContent.nodeType!=1?panelContent.nextSibling:panelContent; 
            
			//clearInterval(panelContent.slideTime);           
            
            if(panelContent.style.display=='block') {
                
				slide(panelContent,-1); 
                
				panelContent.style.display='block'; 
                
			}
        }
    }
    
	//open or close all the tabs in accordion
	slider.prototype.openOrCloseAll=function()
	{
		if(this.isAllOpen){
			this.closeAll();
			this.isAllOpen = false;
		}
        
		else{
			this.openAll();
			this.isAllOpen = true;
		}
	}
    
	//sliding the tab condent smoothly
    
	/*function panelSliding(panel,flag) {
        
		panel.slideTime=setInterval(function(){slide(panel,flag)},slideTime)
        
	}*/
	
	//sliding the tab condent
    
	function slide(panel,flag){
        
        var panelHeight=panel.offsetHeight;
        
		m=panel.maxheight;
        
		clickedPanel=flag==1?m-panelHeight:panelHeight; 
		
		panel.style.height = clickedPanel == 0 ? panelHeight + 'px' : "auto";
        
        if(flag==1) {
            
			//clearInterval(panel.slideTime);
            
		}else if(flag!=1) {
            
			panel.style.display='none'; 
            
            //clearInterval(panel.slideTime);
		}
    }
    
    return{slider:slider}
    
}();